using System.ComponentModel.DataAnnotations;

namespace SocketServer.Entities;

public class Transference
{
    [Key] 
    public int idTranference { get; set; }
    public User User { get; set; }
    public Device DeviceOrigin { get; set; }
    public Device DeviceDestination { get; set; }
    public string FileExtention { get; set; }
    public string FileName { get; set; }
    public long Size { get; set; }
    public string DestinationPath { get; set; }
}