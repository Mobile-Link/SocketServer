using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocketServer.Entities;
using SocketServer.Models;
using SocketServer.Services;

namespace SocketServer.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class UserController(UserService userService, IHttpContextAccessor httpContextAccessor, DeviceService deviceService ) : ControllerBase
{
    [HttpGet("getUsers")]
    public async Task<IActionResult> GetUsers()
    {
        var users = await userService.GetUsers();
        return Ok(users);
    }

    [HttpGet("getUser")]
    public async Task<IActionResult> GetUser()
    {
        var idClaim = httpContextAccessor.HttpContext.User.FindFirst("IdDevice");

        if (idClaim == null)
        {
            return new StatusCodeResult(500);
        }
        var user = deviceService.GetUserByDevice(int.Parse(idClaim.Value));
        if (user == null) {
            return NotFound();       
        }
        return Ok(user);
    }

    [HttpDelete("deleteUser")]
    public async Task<IActionResult> DeleteUser()
    {
        var idClaim = httpContextAccessor.HttpContext.User.FindFirst("IdDevice");
        
        if (idClaim == null)
        {
            return new StatusCodeResult(500);
        }
        
        return await userService.DeleteUser((int.Parse(idClaim.Value)));
    }
    
    [HttpPut("updateUser")]
    public async Task<IActionResult> UpdateUser(UpdateUser request)
    {
        var idClaim = httpContextAccessor.HttpContext.User.FindFirst("IdDevice");
        
        if (idClaim == null)
        {
            return new StatusCodeResult(500);
        }
        
        return await userService.UpdateUser((int.Parse(idClaim.Value)), request);
    }
    
    [HttpPut ("updatePassword")]
    public async Task<IActionResult> UpdatePassword(UpdatePassword request)
    {
        var idClaim = httpContextAccessor.HttpContext.User.FindFirst("IdDevice");
        
        if (idClaim == null)
        {
            return new StatusCodeResult(500);
        }
        
        return await userService.UpdatePassword((int.Parse(idClaim.Value)), request);
    }
}

