using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using SocketServer.Entities;
using SocketServer.Infra;

namespace SocketServer.Services;

public class AuthService(UserService userService)
{

    public async Task<IActionResult> ValidateCredentials(string emailOrUsername ,string password)
    {
        var user = await FindUser(emailOrUsername);

        if (user == null)
        {
            return new NotFoundObjectResult(new { error = "Usuário não encontrado" });
        }
            

        if (!BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            return new UnauthorizedObjectResult(new { error = "Credenciais inválidas" });
        var token = GenerateCode.GenerateJwtToken(user.Email);
        return new OkObjectResult(new { token });

    }

    private async Task<User> FindUser(string emailOrUsername)
    {
        if (new EmailAddressAttribute().IsValid(emailOrUsername))
        {
            return await userService.GetUserByEmail(emailOrUsername);
        }
        
        return await userService.GetUserByUsername(emailOrUsername);
    }
}