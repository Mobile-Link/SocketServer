using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SocketServer.Entities;
using SocketServer.Enums;

namespace SocketServer.Entities;

public class Device
{
    [Key] 
    public int IdDevice { get; set; }
    [ForeignKey("idUser")]
    public User User { get; set; }
    public bool IsDeleted { get; set; }
    public string LastLocation { get; set; }
    public long AvailableSpace { get; set; }
    public long OccupiedSpace { get; set; }
    public string Name { get; set; }
    public string Token { get; set; }
    public DateTime CreationDate { get; set; }
    public DateTime AlterationDate { get; set; }
    [ForeignKey("enDeviceOS")]
    public EnDeviceOSType EnDeviceOsType { get; set; }
}