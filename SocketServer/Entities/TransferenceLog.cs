using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SocketServer.Entities;

public class TransferenceLog
{
    [Key]
    public int idTransferenceLog { get; set; } 
    [ForeignKey("idTransference")]
    public Transference Transference { get; set; }
    [ForeignKey("enStatus")]
    public EnStatus EnStatus { get; set; }
}