using System.ComponentModel.DataAnnotations;

namespace SocketServer.Entities;

public class EntitieChunkStatus
{
    [Key]
    public int IdChunkStatus { get; set; }
    public string Description { get; set; }
}