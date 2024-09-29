using System.ComponentModel.DataAnnotations;

namespace SocketServer.Entities;

public class EnAction
{
    [Key]
    public int idAction { get; set; }
    public string description { get; set; }
}