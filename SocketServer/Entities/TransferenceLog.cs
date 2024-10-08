using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SocketServer.Enums;

namespace SocketServer.Entities;

public class TransferenceLog
{
    [Key]
    public int IdTransferenceLog { get; set; } 
    [ForeignKey("IdTransference")]
    public Transference Transference { get; set; }
    [ForeignKey("EnStatus")]
    public EnStatus EnStatus { get; set; }
    public DateTime Date { get; set; }
    public string ServePath { get; set; }
}