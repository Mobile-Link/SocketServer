using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SocketServer.Enums;

namespace SocketServer.Entities;

public class History
{
    [Key]
    public int IdHistory { get; set; }
    [ForeignKey("IdUser")]
    public User User { get; set; }
    public int IdUser { get; set; }
    [ForeignKey("IdDevice")]
    public Device Device { get; set; }
    public int IdDevice { get; set; }
    [ForeignKey("IdAction")]
    public EntitieAction EnAction { get; set; }
    public int IdAction { get; set; }
    public string Description { get; set; }
    public DateTime Date { get; set; }
    
}