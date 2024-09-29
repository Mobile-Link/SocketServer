using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SocketServer.Entities;

public class AccessLog
{
    [Key]
    public int idAccessLog { get; set; }
    [ForeignKey("idUser")]
    public User User { get; set; }
    [ForeignKey("idDevice")]
    public Device Device { get; set; }
    public DateTime Date { get; set; }
    public string AccessLocation { get; set; }
}