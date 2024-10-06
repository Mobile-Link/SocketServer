using Microsoft.AspNetCore.Mvc;
using SocketServer.Models;
using SocketServer.Services;

namespace SocketServer.Controllers;

[Route("api/[controller]")]
[ApiController]

public class AuthController(AuthService authService, UserService userService, EmailService emailService, VerificationCodeService verificationCodeService) : ControllerBase
{
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] Login request)
    {
        return await authService.ValidateCredentials(request.EmailOrUsername, request.Password);
    }
    
    [HttpPost("sendCode")]
    public async Task<IActionResult> SendCode(string email)
    {
        var verificationCode = verificationCodeService.GenerateVerificationCode();
        
        await verificationCodeService.StoreVerificationCode(email, verificationCode);
        
        await emailService.SendVerificationEmailAsync(email, verificationCode);

        return Ok(new { message = "Verifique seu email para ativar a sua conta" });
    }

    [HttpPost("verifyCode")]
    public async Task<IActionResult> VerifyCode(string email, string code)
    {
        return await verificationCodeService.ValidateVerificationCode(email, code);
    }
    
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] Register request, string code)
    {
        return await userService.Register(request, code);
    }
    
    [HttpPost("forgotPassword")]
    public async Task<IActionResult> ForgotPassword(string email, UpdatePassword request)
    {
        return await userService.UpdatePassword(email, request);
    }
}