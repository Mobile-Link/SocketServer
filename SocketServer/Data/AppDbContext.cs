using Microsoft.EntityFrameworkCore;
using SocketServer.Entities;
using SocketServer.Enums;

namespace SocketServer.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
    
    public DbSet<User> Users { get; set; } // service e controller feita
    //
    public DbSet<VerificationCode> VerificationCodes { get; set; } // service e controller feita
    //
    public DbSet<Device> Devices { get; set; }
    //
    public DbSet<Storage> Storages { get; set; }
    //
    public DbSet<Transference> Transfers { get; set; } // service feita TODO - controller
    public DbSet<TransferenceLog> TransferenceLogs { get; set; }
    //
    public DbSet<AccessLog> AccessLogs { get; set; }
    //
    public DbSet<EnAction> EnActions { get; set; }
    public DbSet<EnDeviceOS> EnDeviceOSs { get; set; }
    public DbSet<EnStatus> EnStatuses { get; set; }
    //
    public DbSet<History> History { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Device>()
            .Property(d => d.EnDeviceOsType)
            .HasConversion<int>();
        
        
        modelBuilder.Entity<TransferenceLog>()
            .Property(t => t.EnStatusType)
            .HasConversion<int>();
        
        
        modelBuilder.Entity<History>()
            .Property(h => h.EnActionType)
            .HasConversion<int>();
        
        
    }

}