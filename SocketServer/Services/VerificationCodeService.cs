using System.Text.Json;
using SocketServer.Entities;

namespace SocketServer.Services;

public class VerificationCodeService(CachingService cache)
{
    private static readonly Random Random = new Random();
    private const string Chars = "0123456789";

    public string GenerateVerificationCode()
    {
        return new string(Chars.ToCharArray().OrderBy(s => Random.Next()).Take(6).ToArray());
    }
    public async Task StoreVerificationCode(string email, string code)
    {
        var key = $"code:{email}";
        await cache.SetAsync(key, code);
    }
    public async Task<string?> GetVerificationCode(string email)
    {
        var key = $"code:{email}";
        return await cache.GetAsync(key);
    }
    public async Task DeleteVerificationCode(string email)
    {
        var key = $"code:{email}";
        await cache.RemoveAsync(key);
    }
}