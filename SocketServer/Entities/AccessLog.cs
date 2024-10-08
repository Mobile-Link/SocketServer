using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SocketServer.Entities;

public class AccessLog
{
    [Key]
    public int IdAccessLog { get; set; }
    [ForeignKey("IdUser")]
    public User User { get; set; }
    [ForeignKey("IdDevice")]
    public Device Device { get; set; }
    public DateTime Date { get; set; }
    public string AccessLocation { get; set; }
}