namespace SocketServer.Models;

public class UpdatePassword
{
    public string Password { get; set; }
    public string Email { get; set; }
    public string Code { get; set; }
}