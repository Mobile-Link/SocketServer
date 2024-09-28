using System.ComponentModel.DataAnnotations;

namespace SocketServer.Entities;

public class User
{
    [Key]
    public int IdUser { get; set; }
    public string Email { get; set; }
    public string Username { get; set; }
    public string PasswordHash { get; set; }
    public DateTime CreationDate { get; set; }
}