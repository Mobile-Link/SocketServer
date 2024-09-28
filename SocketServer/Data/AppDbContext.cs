using Microsoft.EntityFrameworkCore;
using SocketServer.Entities;

namespace SocketServer.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
    
    public DbSet<User> Users { get; set; }
    public DbSet<VerificationCode> VerificationCodes { get; set; }
    public DbSet<Device> Devices { get; set; }
    public DbSet<Storage> Storages { get; set; }
    public DbSet<Transference> Transfers { get; set; }
}