using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SocketServer.Entities;

public class Transference
{
    [Key] 
    public int idTranference { get; set; }
    [ForeignKey("idUser")]
    public User User { get; set; }
    [ForeignKey("idDeviceOrigin")]
    public Device DeviceOrigin { get; set; }
    [ForeignKey("idDeviceDestination")]
    public Device DeviceDestination { get; set; }
    public string FileExtention { get; set; }
    public string FileName { get; set; }
    public long Size { get; set; }
    public string DestinationPath { get; set; }
}