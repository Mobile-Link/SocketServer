using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using SocketServer.Data;
using SocketServer.Entities;

namespace SocketServer.Services;

public class VerificationCodeService(ExpirationDbContext context, IConfiguration configuration)
{
    private static readonly Random Random = new Random();
    private const string Chars = "0123456789";

    public string GenerateVerificationCode()
    {
        return new string(Chars.ToCharArray().OrderBy(s => Random.Next()).Take(6).ToArray());
    }

    public async Task StoreVerificationCode(string email, string code)
    {
        var expirationSeconds = long.Parse(configuration["CodeExpirationTimeSeconds"] ?? "600");
        var verificationCode = new VerificationCode()
        {
            Email = email,
            Code = code,
            ExpirationDate = DateTime.Now.AddSeconds(expirationSeconds)
        };
        await context.VerificationCodes.AddAsync(verificationCode);
        await context.SaveChangesAsync();
    }

    public async Task<VerificationCode?> GetVerificationCode(string email)
    {
        return await context.VerificationCodes.FirstOrDefaultAsync(x => x.Email == email);
    }

    public async Task DeleteVerificationCode(string email)
    {
        var codeToDelete = context.VerificationCodes.FirstOrDefault(x => x.Email == email);
        if (codeToDelete == null)
        {
            return;
        }
        context.VerificationCodes.Remove(codeToDelete);
        await context.SaveChangesAsync();
    }
}