using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SocketServer.Enums;

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
    public EnActionsType EnActionType { get; set; }
    public string Description { get; set; }
    public DateTime Date { get; set; }
    
}