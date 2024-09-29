using System.ComponentModel.DataAnnotations;

namespace SocketServer.Entities;

public class EnDeviceOS
{
    [Key]
    public int idDeviceOS { get; set; }
    public string description { get; set; }
}