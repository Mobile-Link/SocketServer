using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocketServer.Entities;
using SocketServer.Models;
using SocketServer.Services;

namespace SocketServer.Controllers;

[Route("api/[controller]")]
[ApiController]

public class AuthController(AuthService authService, UserService userService, DeviceService deviceService, VerificationCodeService verificationCodeService, IHttpContextAccessor httpContextAccessor) : ControllerBase
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
    
    [HttpPost]
    [Route("updateDeviceInformation")]
    [Authorize(Policy = "Authorized")]
    public async Task<IActionResult> UpdateDeviceInformation([FromBody] UpdateDeviceInformation request)
    {
        var idClaim = httpContextAccessor.HttpContext.User.FindFirst("IdDevice");
        if (idClaim == null)
        {
            return new StatusCodeResult(500);
        }
        //TODO check if user was deleted
        if (request.IdDevice != int.Parse(idClaim.Value))
        {
            return new StatusCodeResult(406);
        }
        var device = deviceService.GetDeviceById(int.Parse(idClaim.Value));
        if (device == null)
        {
            return new StatusCodeResult(404);
        }

        if (device.IsDeleted)
        {
            return new StatusCodeResult(410);
        }
        device.AvailableSpace = request.AvailableSpace; 
        device.OccupiedSpace = request.OccupiedSpace;
        
        deviceService.RegisterAccess(device).ContinueWith(_ => { });
        
        return new OkObjectResult(device);
    }
    
    [HttpPost]
    [Route("sendCode")]
    public async Task<IActionResult> SendCode([FromBody] string email)
    {
        await userService.SendCode(email);
        return Ok(new { message = "Verifique seu email para ativar a sua conta" });
    }
    
    [HttpGet]
    [Route("sendCodeNewAccount")]
    public async Task<IActionResult> sendCodeNewAccount([FromQuery] string email)
    {
        var existingUser = await userService.GetUserByEmail(email);
        if (existingUser != null)
        {
            return BadRequest();
        }

        await userService.SendCode(email);
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
    
    // [HttpPost]
    // [Route("forgotPassword")]
    // public async Task<IActionResult> ForgotPassword(UpdatePassword request)
    // {
    //     var idClaim = httpContextAccessor.HttpContext.User.FindFirst("IdDevice");
    //     
    //     if (idClaim == null)
    //     {
    //         return new StatusCodeResult(500);
    //     }
    //     
    //     return await userService.UpdatePassword((int.Parse(idClaim.Value)), request);
    // }
}