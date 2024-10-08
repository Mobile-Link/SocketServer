using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SocketServer.Entities;

public class Storage
{
    [Key] 
    public int IdStorage { get; set; }
    [ForeignKey("IdUser")]
    public User User { get; set; }
    public long StorageLimitBytes { get; set; }
    public long UsedStorageBytes { get; set; }
}