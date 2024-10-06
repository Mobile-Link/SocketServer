using Microsoft.EntityFrameworkCore;
using SocketServer.Entities;
using SocketServer.Enums;

namespace SocketServer.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; } // service e controller feita
    //
    public DbSet<Device> Devices { get; set; } // TODO fazer controller e service
    //
    public DbSet<Storage> Storages { get; set; } // TODO fazer controller e service
    //
    public DbSet<Transference> Transfers { get; set; } // service feita, TODO - controller
    public DbSet<TransferenceLog> TransferenceLogs { get; set; }
    //
    public DbSet<AccessLog> AccessLogs { get; set; } // TODO fazer controller e service
    //
    public DbSet<EnAction> EnActions { get; set; } // enums criados
    public DbSet<EnDeviceOS> EnDeviceOSs { get; set; } // enums criados
    public DbSet<EnStatus> EnStatuses { get; set; } // enums criados
    //
    public DbSet<History> Histories { get; set; } // TODO fazer controller e service

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