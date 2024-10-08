using System.ComponentModel.DataAnnotations;

namespace SocketServer.Entities;

public class EntitieDeviceOS
{
    [Key]
    public int IdDeviceOs { get; set; }
    public string Description { get; set; }
}