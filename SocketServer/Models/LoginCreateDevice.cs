using System.ComponentModel.DataAnnotations;

namespace SocketServer.Models;

public class LoginCreateDevice
{
    [Required]
    public string EmailOrUsername { get; set; }
    
    [Required]
    public string Password { get; set; }
        
    [Required]
    public string DeviceName { get; set; }
    
    [Required]
    public string Code { get; set; }
}