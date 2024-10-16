using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SocketServer.Entities;

public class Transference
{
    [Key] 
    public int IdTranference { get; set; }
    [ForeignKey("IdUser")]
    public User User { get; set; }
    public int IdUser { get; set; }
    
    [ForeignKey("IdDeviceOrigin")]
    public Device DeviceOrigin { get; set; }
    public int IdDeviceOrigin { get; set; }
    [ForeignKey("IdDeviceDestination")]
    public Device DeviceDestination { get; set; }
    public int IdDeviceDestination { get; set; }
    public string FilePath { get; set; }
    public long Size { get; set; }
    public string DestinationPath { get; set; }
}