using System.ComponentModel.DataAnnotations;

namespace SocketServer.Entities;

public class EntitieAction
{
    [Key]
    public int IdAction { get; set; }
    public string Description { get; set; }
}