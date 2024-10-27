using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SocketServer.Data;
using SocketServer.Models;
using SocketServer.Entities;
using SocketServer.Enums;

namespace SocketServer.Services;

public class UserService(AppDbContext context, VerificationCodeService verificationCodeService, DeviceService deviceService, HistoryService historyService)
{
    public async Task<IActionResult> Register(Register request)
    {
        if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Password))
        {
            return new BadRequestObjectResult( new{ error = "Email e senha são obrigatórios" });
        }

        if (!new EmailAddressAttribute().IsValid(request.Email))
        {
            return new BadRequestObjectResult(new {error = "Formato de email inválido"});
        }
        
        //TODO - Fazer um dicionário de erros
        
        var existingUser = await context.Users.FirstOrDefaultAsync(u => u.Email == request.Email || u.Username == request.Username);
        
        //TODO - Talvez fazer rotas de validação de email e username existentes
        
        if (existingUser != null)
        {
            return new BadRequestObjectResult(new {error = "Email ou Usuário já cadastrado"});
        }
        
        var result = await verificationCodeService.ValidateVerificationCode(request.Email, request.Code);
        if (!result)
        {
            return new BadRequestObjectResult(new {error = "Código inválido ou expirado"});;
        }
        var user = new User
        {
            Email = request.Email,
            Username = request.Username,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
            CreationDate = DateTime.Now,
        };
        context.Users.Add(user);
        await context.SaveChangesAsync();
        
        var device = await deviceService.CreateDevice(user, request.DeviceName);
        
        await verificationCodeService.DeleteVerificationCode(request.Email);
        
        var token = await deviceService.CreateDeviceToken(device.IdDevice);

        return new OkObjectResult( new { message = "Usuário cadastrado com sucesso", token.Token, device.IdDevice});
    }

    public async Task<IActionResult> DeleteUser(int idDevice)
    {
        var user = deviceService.GetUserByDevice(idDevice);
        
        if (user == null)
        {
            return new NotFoundObjectResult(new {error = "Usuário não encontrado"});
        }
        
        context.Users.Remove(user);
        await context.SaveChangesAsync();
        
        return new OkObjectResult(new {message = "Usuário removido com sucesso"});
    }
    
    public async Task<IActionResult> UpdateUser(int idDevice, UpdateUser request)
    {
        var user = deviceService.GetUserByDevice(idDevice);
        
        if (user == null)
        {
            return new NotFoundObjectResult(new {error = "Usuário não encontrado"});
        }
        
        if(!string.IsNullOrEmpty(request.Username))
        {
            user.Username = request.Username;
        }

        context.Users.Update(user);
        
        await context.SaveChangesAsync();
        context.Entry(user).State = EntityState.Detached;
        await historyService.CreateHistory(
            EnActions.ChangedUser,
            $"O usuário modificou o nome para {user.Username}",
            idDevice,
            user.IdUser
        );
        
        return new OkObjectResult(new { message = "Usuário atualizado com sucesso" });
    }
    
    public async Task<IActionResult> UpdatePassword(int idDevice, UpdatePassword request)
    {
        var user = deviceService.GetUserByDevice(idDevice);
        
        if (user == null)
        {
            return new NotFoundObjectResult(new {error = "Usuário não encontrado"});
        }
        
        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);
        
        context.Users.Update(user);
        await context.SaveChangesAsync();
        
        await historyService.CreateHistory(
            EnActions.ChangedPassword,
            $"Senha do usuário {user.Username} alterada",
            idDevice,
            user.IdUser
        );
        
        return new OkObjectResult(new {message = "Senha atualizada com sucesso"});
    }
    
    public async Task<List<User>> GetUsers()
    {
        return await context.Users.ToListAsync();
    }
    
    public async Task<User?> GetUserByEmail(string email)
    {
        var user = await context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Email == email);
        
        return user;
    }
    
    public async Task<User?> GetUserByUsername(string username)
    {
        var user = await context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Username == username);
        
        return user;
    }
}