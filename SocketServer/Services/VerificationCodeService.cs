using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using SocketServer.Data;
using Microsoft.AspNetCore.Mvc;
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
        var verificationCode = new VerificationCode()
        {
            Email = email,
            Code = code,
            InsertionDate = DateTime.Now
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
    
    public async Task<IActionResult> ValidateVerificationCode(string email, string code)
    {
        var storedCode = await GetVerificationCode(email);
        
        if (storedCode == null || storedCode?.Code != code)
        {
            return new BadRequestObjectResult(new {error = "Código inválido"});;
        }
        else
        {
            return new OkObjectResult(new { message = "Código validado com sucesso" });
        }
    }
}