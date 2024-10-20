using Microsoft.AspNetCore.Authorization;
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
        return await authService.Login(request);
    }
    [HttpPost]
    [Route("loginCreateDevice")]
    public async Task<IActionResult> LoginCreateDevice([FromBody] LoginCreateDevice request)
    {
        return await authService.LoginCreateDevice(request);
    }
    [HttpPost]
    [Route("validateCredentials")]
    public async Task<IActionResult> ValidateCredentials([FromBody] ValidateCredentials request)
    {
        return await authService.ValidateCredentials(request.EmailOrUsername, request.Password);
    }
    
    [HttpGet]
    [Route("verifyToken")]
    [Authorize(Policy = "Authorized")]
    public IActionResult VerifyToken()
    {
        return new OkResult();
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
        var result = await verificationCodeService.ValidateVerificationCode(email, code);
        if (!result)
        {
            return new BadRequestObjectResult(new {error = "Código inválido"});;
        }
        return new OkObjectResult(new { message = "Código validado com sucesso" });
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