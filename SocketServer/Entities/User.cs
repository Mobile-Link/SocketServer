using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace SocketServer.Entities;

[Index(nameof(Email), IsUnique = true)]
[Index(nameof(Username),  IsUnique = true)]
public class User
{
    [Key]
    public int IdUser { get; set; }
    public string Email { get; set; }
    public string Username { get; set; }
    public string PasswordHash { get; set; }
    public DateTime CreationDate { get; set; }
}