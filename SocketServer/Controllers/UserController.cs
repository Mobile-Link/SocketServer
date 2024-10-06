using Microsoft.AspNetCore.Mvc;
using SocketServer.Models;
using SocketServer.Services;

namespace SocketServer.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController(UserService userService) : ControllerBase
{
    [HttpGet("user")]
    public async Task<IActionResult> GetUser()
    {
        var users = await userService.GetUsers();
        return Ok(users);
    }
    
    [HttpDelete("user/{IdUser}")]
    public async Task<IActionResult> DeleteUser(int idUser)
    {
        return await userService.DeleteUser(idUser);
    }
    
    [HttpPut("user/{IdUser}")]
    public async Task<IActionResult> UpdateUser(int idUser, UpdateUser request)
    {
        return await userService.UpdateUser(idUser, request);
    }
    
    [HttpPut ("user/{IdUser}/password")]
    public async Task<IActionResult> UpdatePassword(string email, UpdatePassword request)
    {
        return await userService.UpdatePassword(email, request);
    }
}