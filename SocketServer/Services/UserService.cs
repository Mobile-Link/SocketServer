using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SocketServer.Data;
using SocketServer.Models;
using SocketServer.Entities;
using SocketServer.Infra;

namespace SocketServer.Services;

public class UserService(AppDbContext context, VerificationCodeService verificationCodeService)
{
    public async Task<IActionResult> Register(Register request, string code)
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
        
        var existingUser = await context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
        
        if (existingUser != null)
        {
            return new BadRequestObjectResult(new {error = "Email já cadastrado"});
        }
        
        var storedCode = await verificationCodeService.GetVerificationCode(request.Email);
        
        if (storedCode == null || storedCode?.Code != code)
        {
           return new BadRequestObjectResult(new { error = "Código inválido ou expirado" });
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
        
        await verificationCodeService.DeleteVerificationCode(request.Email);
        var token = GenerateCode.GenerateJwtToken(request.Email);

        return new OkObjectResult( new { message = "Usuário cadastrado com sucesso", token});
    }

    public async Task<IActionResult> DeleteUser(int IdUser)
    {
        var user = await context.Users.FindAsync(IdUser);
        
        if (user == null)
        {
            return new NotFoundObjectResult(new {error = "Usuário não encontrado"});
        }
        
        context.Users.Remove(user);
        await context.SaveChangesAsync();
        
        return new OkObjectResult(new {message = "Usuário removido com sucesso"});
    }
    
    public async Task<IActionResult> UpdateUser(int IdUser, UpdateUser request)
    {
        var user = await context.Users.FindAsync(IdUser);
        
        if (user == null)
        {
            return null;
        }
        
        if(!string.IsNullOrEmpty(request.Username))
        {
            user.Username = request.Username;
        }

        if (!string.IsNullOrEmpty(request.Email))
        {
            user.Email = request.Email;
        }

        context.Users.Update(user);
        await context.SaveChangesAsync();
        
        return new OkObjectResult(new { message = "Usuário atualizado com sucesso" });
    }
    
    public async Task<IActionResult> UpdatePassword(string email, UpdatePassword request)
    {
        var user = await context.Users.FindAsync(email);
        
        if (user == null)
        {
            return new NotFoundObjectResult(new {error = "Usuário não encontrado"});
        }
        
        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);
        
        context.Users.Update(user);
        await context.SaveChangesAsync();
        
        return new OkObjectResult(new {message = "Senha atualizada com sucesso"});
    }
    
    public async Task<List<User>> GetUsers()
    {
        return await context.Users.ToListAsync();
    }
    
    public async Task<User> GetUserByEmail(string email)
    {
        var user = await context.Users.FirstOrDefaultAsync(u => u.Email == email);
        
        return user;
    }
    
    public async Task<User> GetUserByUsername(string username)
    {
        var user = await context.Users.FirstOrDefaultAsync(u => u.Username == username);
        
        return user;
    }
}