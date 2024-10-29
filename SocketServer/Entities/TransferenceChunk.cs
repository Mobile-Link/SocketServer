using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SocketServer.Enums;

namespace SocketServer.Entities;

public class TransferenceChunk
{
    [Key] 
    public int IdTranferenceChunck { get; set; }
    [ForeignKey("IdTransference")]
    public Transference Transference { get; set; }
    public int IdTransference { get; set; }
    public long startByteIndex { get; set; }
    [ForeignKey("EnChunkStatus")]
    public EnChunkStatus EnChunkStatus { get; set; }
}