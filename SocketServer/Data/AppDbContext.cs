using Microsoft.EntityFrameworkCore;
using SocketServer.Entities;

namespace SocketServer.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
    public DbSet<VerificationCode> VerificationCodes { get; set; }
    public DbSet<Device> Devices { get; set; }
    public DbSet<Storage> Storages { get; set; }
    public DbSet<Transference> Transfers { get; set; }
    public DbSet<AccessLog> AccessLogs { get; set; }
    public DbSet<EnAction> EnActions { get; set; }
    public DbSet<EnDeviceOS> EnDeviceOSs { get; set; }
    public DbSet<EnStatus> EnStatuses { get; set; }
    public DbSet<History> History { get; set; }
    public DbSet<TransferenceLog> TransferenceLogs { get; set; }
}