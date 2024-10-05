using Microsoft.AspNetCore.Mvc;
using SocketServer.Entities;
using SocketServer.Models;
using SocketServer.Services;

namespace SocketServer.Controllers;

[Route("api/[controller]")]
[ApiController]

public class AuthController(AuthService authService, UserService userService, EmailService emailService, VerificationCodeService verificationCodeService) : ControllerBase
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

        var verificationCode = verificationCodeService.GenerateVerificationCode();
        
        await verificationCodeService.StoreVerificationCode(request.Email, verificationCode);
        
        await emailService.SendVerificationEmailAsync(request.Email, verificationCode);

        return Ok(new { message = "Verifique seu email para ativar a sua conta" });
    }

    [HttpPost("verifyCode")]
    public async Task<IActionResult> VerifyCode(string email, string code, Register request)
    {
        var storedCode = await verificationCodeService.GetVerificationCode(email);
        
        if (storedCode == null || storedCode != code)
        {
            return BadRequest(new { error = "Código inválido ou expirado" });
        }
        
        var registerResult = await userService.Register(request);
        
        await userService.ActivateUser(email);
        
        if (registerResult is OkObjectResult)
        {
            await verificationCodeService.DeleteVerificationCode(email);
            var token = authService.GenerateJwtToken(email);
            return Ok(new { token });
        }
        else
        {
            return BadRequest(registerResult);
        }
    }
}