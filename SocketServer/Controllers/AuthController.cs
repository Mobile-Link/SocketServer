using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using SocketServer.Models;
using SocketServer.Services;

namespace SocketServer.Controllers;

[Route("api/[controller]")]
[ApiController]

public class AuthController : ControllerBase
{
    private readonly AuthService _authService;
    private readonly UserService _userService;
    
    public AuthController(AuthService authService, UserService userService)
    {
        _authService = authService;
        _userService = userService;
    }

    [HttpPost("login")]
    public IActionResult Login(string email, string password)
    {
        if(_authService.ValidateCredentials(email, password))
        {
            var token = _authService.GenerateJwtToken(email);
            
            return Ok(new { token });
        }
        else
        {
            return Unauthorized("Credenciais inválidas");
        }
    }  
    
    [HttpPost("register")]
    public IActionResult Register(RegisterRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        var result = _userService.Register(request);
        
        if (result is OkObjectResult)
        {
            var user = (result as OkObjectResult).Value as User;
            var token = _authService.GenerateJwtToken(user.Email);
            return Ok(new {token});
        }
        else
        {
            return BadRequest(result);
        }
    }

    [HttpGet("user")]
    public IActionResult GetUser()
    {
        var users = _userService.GetUsers();
        return Ok(users);
    }
}