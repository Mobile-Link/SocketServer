using System.ComponentModel.DataAnnotations;

namespace SocketServer.Models;

public class ValidateCredentials
{
    [Required]
    public string EmailOrUsername { get; set; }
    
    [Required]
    public string Password { get; set; }

}