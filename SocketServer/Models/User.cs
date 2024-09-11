using System.ComponentModel.DataAnnotations;

namespace SocketServer.Models;

public class User
{
    [Key]
    public int IdUser { get; set; }
    public string Email { get; set; }
    public string Username { get; set; }
    public string PasswordHash { get; set; }
    public DateTime CreationDate { get; set; }
    public bool IsActive { get; set; }
    public string VerificationCode { get; set; }
}