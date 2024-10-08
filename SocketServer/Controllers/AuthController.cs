using Microsoft.AspNetCore.Mvc;
using SocketServer.Models;
using SocketServer.Services;

namespace SocketServer.Controllers;

[Route("api/[controller]")]
[ApiController]

public class AuthController(AuthService authService, UserService userService, EmailService emailService, VerificationCodeService verificationCodeService) : ControllerBase
{
    [HttpPost]
    [Route("login")]
    public async Task<IActionResult> Login([FromBody] Login request)
    {
        return await authService.ValidateCredentials(request.EmailOrUsername, request.Password);
    }
    
    [HttpPost]
    [Route("sendCode")]
    public async Task<IActionResult> SendCode(string email)
    {
        var verificationCode = verificationCodeService.GenerateVerificationCode();
        
        await verificationCodeService.StoreVerificationCode(email, verificationCode);
        
        await emailService.SendVerificationEmailAsync(email, verificationCode);

        return Ok(new { message = "Verifique seu email para ativar a sua conta" });
    }

    [HttpPost]
    [Route("verifyCode")]
    public async Task<IActionResult> VerifyCode(string email, string code)
    {
        return await verificationCodeService.ValidateVerificationCode(email, code);
    }
    
    [HttpPost]
    [Route("register")]
    
    public async Task<IActionResult> Register([FromBody] Register request)
    {
        return await userService.Register(request);
    }
    
    [HttpPost]
    [Route("forgotPassword")]
    public async Task<IActionResult> ForgotPassword(string email, UpdatePassword request)
    {
        return await userService.UpdatePassword(email, request);
    }
}