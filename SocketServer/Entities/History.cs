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
    [ForeignKey("IdDevice")]
    public Device Device { get; set; }
    [ForeignKey("EnAction")]
    public EnActions EnAction { get; set; }
    public string Description { get; set; }
    public DateTime Date { get; set; }
    
}