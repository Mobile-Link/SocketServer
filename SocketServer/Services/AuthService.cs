using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace SocketServer.Services;

public class AuthService(IConfiguration configuration)
{
    private readonly IConfiguration _configuration = configuration;

    public bool ValidateCredentials(string user, string password)
    {
        if (user == "usuário" && password == "senha")
        {
            return true;
        }

        return false;
    }

    public string GenerateJwtToken(string user)
    {
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        };
    
        var signingCredentials = new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes("MobileLinkSecretMobileLinkSecret")),
            SecurityAlgorithms.HmacSha256);
    
        var token = new JwtSecurityToken(
            issuer: "MobileLink",
            audience: "MobileLink",
            claims: claims,
            expires: DateTime.UtcNow.AddHours(3),
            signingCredentials: signingCredentials
        );
    
        var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
        return tokenString;
    }
}