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
        var result = await userService.DeleteUser(idUser);
        return result;
    }
    
    [HttpPut("user/{IdUser}")]
    public async Task<IActionResult> UpdateUser(int idUser, UpdateUser request)
    {
        var result = await userService.UpdateUser(idUser, request);
        return result;
    }
    
    [HttpPut ("user/{IdUser}/password")]
    public async Task<IActionResult> UpdatePassword(int idUser, UpdatePassword request)
    {
        var result = await userService.UpdatePassword(idUser, request);
        return result;
    }
}