using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using SocketServer.Entities;
using SocketServer.Infra;
using SocketServer.Models;

namespace SocketServer.Services;

public class AuthService(UserService userService, DeviceService deviceService, VerificationCodeService verificationCodeService, EmailService emailService)
{

    public async Task<IActionResult> Login(Login loginRequest)
    {
        var user = await FindUser(loginRequest.EmailOrUsername);

        if (user == null)
        {
            return new NotFoundObjectResult(new { error = "Usuário não encontrado" });
        }
        
        if (!BCrypt.Net.BCrypt.Verify(loginRequest.Password, user.PasswordHash))
            return new UnauthorizedObjectResult(new { error = "Credenciais inválidas" });

        var device = deviceService.GetDeviceById(loginRequest.IdDevice);

        if (device == null)
        {
             return new NotFoundObjectResult(new { error = "Dispositivo não encontrado" });
        }
        
        var token = await deviceService.CreateDeviceToken(loginRequest.IdDevice);

        deviceService.RegisterAccess(device).ContinueWith(_ => { });
        
        return new OkObjectResult(new { token.Token });
    }
    
    public async Task<IActionResult> LoginCreateDevice(LoginCreateDevice loginRequest)
    {
        var user = await FindUser(loginRequest.EmailOrUsername);

        if (user == null)
        {
            return new NotFoundObjectResult(new { error = "Usuário não encontrado" });
        }
        
        if (!BCrypt.Net.BCrypt.Verify(loginRequest.Password, user.PasswordHash))
            return new UnauthorizedObjectResult(new { error = "Credenciais inválidas" });
        //TODO validate if there is another device with the same name
        var device = await deviceService.CreateDevice(user, loginRequest.DeviceName);
        var token = await deviceService.CreateDeviceToken(device.IdDevice);

        return new OkObjectResult(new { token.Token, device.IdDevice });
    }
    
    public async Task<IActionResult> ValidateCredentials(string emailOrUsername ,string password)
    {
        var user = await FindUser(emailOrUsername);

        if (user == null)
        {
            return new NotFoundObjectResult(new { error = "Usuário não encontrado" });
        }
            

        if (!BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            return new UnauthorizedObjectResult(new { error = "Credenciais inválidas" });
        
        var verificationCode = verificationCodeService.GenerateVerificationCode();
        
        await verificationCodeService.StoreVerificationCode(user.Email, verificationCode);
        
        await emailService.SendVerificationEmailAsync(user.Email, verificationCode);
        return new OkResult();

    }

    private async Task<User?> FindUser(string emailOrUsername)
    {
        if (new EmailAddressAttribute().IsValid(emailOrUsername))
        {
            return await userService.GetUserByEmail(emailOrUsername);
        }
        
        return await userService.GetUserByUsername(emailOrUsername);
    }
}