using Microsoft.AspNetCore.Mvc;
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

        return Ok(new { message = "Verifique seu email para ativar sa conta" });
    }

    [HttpPost("verifyCode")]
    public async Task<IActionResult> VerifyCode(string email, string code, Register request)
    {
        var storedCode = await userService.GetVerificationCode(email);
        
        if (storedCode == null || storedCode != code)
        {
            return BadRequest(new { error = "Código inválido" });
        }
        
        var registerResult = await userService.Register(request);
        
        await userService.ActivateUser(email);
        
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

    [HttpGet("user")]
    public async Task<IActionResult> GetUser()
    {
        var users = await userService.GetUsers();
        return Ok(users);
    }
    
    [HttpDelete("user/{IdUser}")]
    public async Task<IActionResult> DeleteUser(int IdUser)
    {
        var result = await userService.DeleteUser(IdUser);
        return result;
    }
    
    [HttpPut("user/{IdUser}")]
    public async Task<IActionResult> UpdateUser(int IdUser, UpdateUser request)
    {
        var result = await userService.UpdateUser(IdUser, request);
        return result;
    }
    
    [HttpPut ("user/{IdUser}/password")]
    public async Task<IActionResult> UpdatePassword(int IdUser, UpdatePassword request)
    {
        var result = await userService.UpdatePassword(IdUser, request);
        return result;
    }
}