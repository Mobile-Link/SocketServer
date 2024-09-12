namespace SocketServer.Services;

public class CodeGeneratorService
{
    private static readonly Random _random = new Random();
    private const string _chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

    public string GenerateVerificationCode()
    {
        return new string(_chars.ToCharArray().OrderBy(s => _random.Next()).Take(6).ToArray());
    }
}