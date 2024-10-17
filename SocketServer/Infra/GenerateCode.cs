using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace SocketServer.Infra;

public static class GenerateCode
{
    public static string GenerateJwtToken(string idDevice)
    {
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, idDevice),
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
        
        //TODO - Validar renovação de token
    }
}