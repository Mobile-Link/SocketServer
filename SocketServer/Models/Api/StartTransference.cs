using System.ComponentModel.DataAnnotations;

namespace SocketServer.Models;

public class StartTransference
{
    [Required] public int IdDevice { get; set; }
    [Required] public long FileSize { get; set; }

    [Required] public string FilePath { get; set; }

    [Required] public string DestinationPath { get; set; }
}