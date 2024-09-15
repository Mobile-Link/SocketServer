using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace SocketServer.Services;

public class AuthService(IConfiguration configuration, UserService userService)
{
    private readonly UserService _userService = userService;
    private readonly IConfiguration _configuration = configuration;

    public async Task<bool> ValidateCredentials(string emailOrUsername ,string password)
    {
        if (new EmailAddressAttribute().IsValid(emailOrUsername))
        {
            var user = await _userService.GetUserByEmail(emailOrUsername);
            if (user == null)
            {
                return false;
            }

            if (BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            {
                return true;
            }
        } else 
        {
            var user = await _userService.GetUserByUsername(emailOrUsername);
            if (user == null)
            {
                return false;
            }

            if (BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            {
                return true;
            }
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
        
        //TODO - Validar renovação de token
    }
}