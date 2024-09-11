namespace SocketServer.Services;

public class CodeGeneratorService
{
    private static readonly Random _random = new Random();
    private const string _chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

    public string GenerateVerificationCode()
    {
        int length = _random.Next(6, 6);
        return new string(_chars.ToCharArray().OrderBy(s => _random.Next()).Take(length).ToArray());
    }
}