using System.ComponentModel.DataAnnotations;
using SocketServer.Entities;

namespace SocketServer.Entities;

public class Device
{
    [Key] 
    public int IdDevice { get; set; }
    public User User { get; set; }
    public bool IsDeleted { get; set; }
    public string LastLocation { get; set; }
    public long AvailableSpace { get; set; }
    public long OccupiedSpace { get; set; }
    public string Name { get; set; }
    public string Token { get; set; }
    public DateTime CreationDate { get; set; }
    public DateTime AlterationDate { get; set; }
    //deviceOS
}