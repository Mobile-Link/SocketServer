using System.ComponentModel.DataAnnotations;

namespace SocketServer.Models;

public class Register
{
    [Required]
    public string Username { get; set; }
    
    [Required]
    public string Email { get; set; }
    
    [Required]
    public string Password { get; set; }
    
    [Required]
    public string Code { get; set; }
    
    [Required]
    public string DeviceName { get; set; }
}