using System.ComponentModel.DataAnnotations;

namespace SocketServer.Entities;

public class EntitieStatus
{
    [Key]
    public int IdStatus { get; set; }
    public string Description { get; set; }
}