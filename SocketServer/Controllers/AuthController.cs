using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using SocketServer.Models;
using SocketServer.Services;

namespace SocketServer.Controllers;

[Route("api/[controller]")]
[ApiController]

public class AuthController(AuthService authService, UserService userService) : ControllerBase
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
        
        var result = await userService.Register(request);
        
        if (result is OkObjectResult)
        {
            var user = (result as OkObjectResult).Value as User;
            var token = authService.GenerateJwtToken(user.Email);
            return Ok(new {token});
        }
        else
        {
            return BadRequest(result);
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