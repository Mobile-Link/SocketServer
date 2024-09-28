using System.ComponentModel.DataAnnotations;

namespace SocketServer.Entities;

public class Storage
{
    [Key] 
    public int IdDevice { get; set; }
    public User User { get; set; }
    public long StorageLimitBytes { get; set; }
    public long UsedStorageBytes { get; set; }
}