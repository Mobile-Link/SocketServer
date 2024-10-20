using System.ComponentModel.DataAnnotations;

namespace SocketServer.Models;

public class Login
{
    [Required]
    public string EmailOrUsername { get; set; }
    
    [Required]
    public string Password { get; set; }
        
    [Required]
    public int IdDevice { get; set; }
}