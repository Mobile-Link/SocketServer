using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using SocketServer.Entities;

namespace SocketServer.Data;

public class ExpirationDbContext(DbContextOptions<ExpirationDbContext> options, IConfiguration configuration) : DbContext(options)
{
    public DbSet<VerificationCode> VerificationCodes { get; set; }
    public DbSet<DeviceToken> DeviceTokens { get; set; }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);

        optionsBuilder.UseMongoDB(
            configuration.GetConnectionString("MongoDBConnection"),
            configuration.GetConnectionString("MongoDBDatabaseName")
        );

        CreateExpirationIndex();
    }

    private async Task CreateExpirationIndex()
    {
        var client = new MongoClient(configuration.GetConnectionString("MongoDBConnection"));
        var database = client.GetDatabase(configuration.GetConnectionString("MongoDBDatabaseName"));
        var tasks = new List<Task>();
        tasks.Add(CreteExpirationVerificationCode(database));
        tasks.Add(CreteExpirationDeviceToken(database));
        Task.WaitAll(tasks.ToArray());
    }
    private async Task CreteExpirationVerificationCode(IMongoDatabase database)
    {
        var collection = database.GetCollection<VerificationCode>("VerificationCodes");
        var indexDefinition = Builders<VerificationCode>.IndexKeys.Ascending(x => x.InsertionDate);
        var indexOptions = new CreateIndexOptions { ExpireAfter = TimeSpan.FromSeconds(int.Parse(configuration["CodeExpirationTimeSeconds"] ?? "600"))};
        var indexModel = new CreateIndexModel<VerificationCode>(indexDefinition, indexOptions);
        await collection.Indexes.CreateOneAsync(indexModel);
    }
    private async Task CreteExpirationDeviceToken(IMongoDatabase database)
    {
        var collection = database.GetCollection<DeviceToken>("DeviceTokens");
        var indexDefinition = Builders<DeviceToken>.IndexKeys.Ascending(x => x.InsertionDate);
        var indexOptions = new CreateIndexOptions { ExpireAfter = TimeSpan.FromDays(int.Parse(configuration["TokenExpirationTimeDays"] ?? "30"))};
        var indexModel = new CreateIndexModel<DeviceToken>(indexDefinition, indexOptions);
        await collection.Indexes.CreateOneAsync(indexModel);
    }
};