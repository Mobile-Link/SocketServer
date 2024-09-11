using System.ComponentModel.DataAnnotations;

namespace SocketServer.Models;

public class VerificationCode
{
    [Key]
    public int IdCode { get; set; }
    public string Code { get; set; }
    public DateTime CreationDate { get; set; }
    public string Email { get; set; }
}