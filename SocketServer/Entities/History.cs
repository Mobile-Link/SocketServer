using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SocketServer.Entities;

public class History
{
    [Key]
    public int idHistory { get; set; }
    [ForeignKey("idUser")]
    public User User { get; set; }
    [ForeignKey("idDevice")]
    public Device Device { get; set; }
    [ForeignKey("enAction")]
    public EnAction EnAction { get; set; }
}