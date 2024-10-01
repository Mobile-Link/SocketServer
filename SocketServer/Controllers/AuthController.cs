using Microsoft.AspNetCore.Mvc;
using SocketServer.Entities;
using SocketServer.Models;
using SocketServer.Services;

namespace SocketServer.Controllers;

[Route("api/[controller]")]
[ApiController]

public class AuthController(AuthService authService, UserService userService, EmailService emailService, CodeGeneratorService codeGeneratorService) : ControllerBase
{
    [HttpPost("login")]
    public async Task<IActionResult> Login(Login request)
    {
        if (await authService.ValidateCredentials(request.EmailOrUsername, request.Password))
        {
            var token = authService.GenerateJwtToken(request.EmailOrUsername);
            return Ok(new { token });
        }
        else
        {
            return Unauthorized(new { error = "Credenciais inválidas" });
        }
    }
    
    [HttpPost("register")]
    public async Task<IActionResult> Register(Register request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var verificationCode = codeGeneratorService.GenerateVerificationCode();
        
        await userService.StoreVerificationCode(request.Email, verificationCode);
        
        await emailService.SendVerificationEmailAsync(request.Email, verificationCode);

        return Ok(new { message = "Verifique seu email para ativar a sua conta" });
    }

    [HttpPost("verifyCode")]
    public async Task<IActionResult> VerifyCode(string email, string code, Register request)
    {
        var storedCode = await userService.GetVerificationCode(email);
        
        if (storedCode == null || storedCode != code)
        {
            return BadRequest(new { error = "Código inválido ou expirado" });
        }
        
        var registerResult = await userService.Register(request);
        
        if (registerResult is OkObjectResult)
        {
            await userService.DeleteVerificationCode(email);
            var user = (registerResult as OkObjectResult).Value as User;
            var token = authService.GenerateJwtToken(email);
            return Ok(new { token });
        }
        else
        {
            return BadRequest(registerResult);
        }
    }
}