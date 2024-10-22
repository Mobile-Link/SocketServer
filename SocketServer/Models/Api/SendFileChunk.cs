using System.ComponentModel.DataAnnotations;

namespace SocketServer.Models;

public class SendFileChunk
{
    [Required] public int IdTransfer { get; set; }
    [Required] public long StartByteIndex { get; set; }

    [Required] public byte[] ByteArray { get; set; }
}