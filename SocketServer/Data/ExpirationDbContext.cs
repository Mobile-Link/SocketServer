using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using SocketServer.Entities;

namespace SocketServer.Data;

public class ExpirationDbContext(DbContextOptions<ExpirationDbContext> options, IConfiguration configuration) : DbContext(options)
{
    public DbSet<VerificationCode> VerificationCodes { get; set; }
    
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
        var collection = database.GetCollection<VerificationCode>("VerificationCodes");
        var indexDefinition = Builders<VerificationCode>.IndexKeys.Ascending(x => x.InsertionDate);
        var indexOptions = new CreateIndexOptions { ExpireAfter = TimeSpan.FromSeconds(int.Parse(configuration["CodeExpirationTimeSeconds"] ?? "600"))};
        var indexModel = new CreateIndexModel<VerificationCode>(indexDefinition, indexOptions);
        await collection.Indexes.CreateOneAsync(indexModel);
    }
};