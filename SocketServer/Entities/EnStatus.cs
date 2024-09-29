using System.ComponentModel.DataAnnotations;

namespace SocketServer.Entities;

public class EnStatus
{
    [Key]
    public int idStatus { get; set; }
    public string description { get; set; }
}