using System.ComponentModel.DataAnnotations;

namespace SocketServer.Models;

public class UpdateDeviceInformation
{
    [Required] public int IdDevice { get; set; }
    [Required] public long AvailableSpace { get; set; }
    [Required] public long OccupiedSpace { get; set; }
}