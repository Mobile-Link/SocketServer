using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SocketServer.Entities;
using SocketServer.Enums;

namespace SocketServer.Entities;

public class Device
{
    [Key] 
    public int IdDevice { get; set; }
    [ForeignKey("IdUser")]
    public User User { get; set; }
    public int IdUser { get; set; }
    public bool IsDeleted { get; set; }
    public string LastLocation { get; set; }
    public long AvailableSpace { get; set; }
    public long OccupiedSpace { get; set; }
    public string Name { get; set; }
    public DateTime CreationDate { get; set; }
    public DateTime AlterationDate { get; set; }
    [ForeignKey("EnDeviceOS")]
    public EnDeviceOs EnDeviceOs { get; set; }
}