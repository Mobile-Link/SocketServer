using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SocketServer.Enums;

namespace SocketServer.Entities;

public class TransferenceLog
{
    [Key]
    public int idTransferenceLog { get; set; } 
    [ForeignKey("idTransference")]
    public Transference Transference { get; set; }
    [ForeignKey("enStatus")]
    public EnStatusType EnStatusType { get; set; }
}