using System.ComponentModel.DataAnnotations;
using System.Xml.Schema;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SocketServer.Data;
using SocketServer.Models;
using SocketServer.Entities;

namespace SocketServer.Services;

public class UserService(AppDbContext context)
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
        
        var existingUser = await context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
        
        if (existingUser != null)
        {
            return new BadRequestObjectResult(new {error = "Email já cadastrado"});
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

        return new OkObjectResult(user);
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
    
    public async Task<IActionResult> UpdatePassword(int IdUser, UpdatePassword request)
    {
        var user = await context.Users.FindAsync(IdUser);
        
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
    
    public async Task ActivateUser(string email)
    {
        var user = await context.Users.FirstOrDefaultAsync(u => u.Email == email);
        
        if (user != null)
        {
            await context.SaveChangesAsync();
        }
    }
    
    public async Task StoreVerificationCode(string email, string code)
    {
        var verificationCode = new VerificationCode
        {
            Email = email,
            Code = code,
            CreationDate = DateTime.Now,
            ExpirationDate = DateTime.Now.AddMinutes(1)
        };
        
        context.VerificationCodes.Add(verificationCode);
        await context.SaveChangesAsync();
    }
    
    public async Task<string> GetVerificationCode(string email)
    {
        var verificationCode = await context.VerificationCodes.FirstOrDefaultAsync(vc => vc.Email == email);
        if (verificationCode != null && verificationCode.ExpirationDate >= DateTime.Now)
        {
            return verificationCode.Code;
        }

        return null;
    }
    
    public async Task DeleteVerificationCode(string email)
    {
        var verificationCode = await context.VerificationCodes.FirstOrDefaultAsync(vc => vc.Email == email);
        if (verificationCode != null)
        {
            context.VerificationCodes.Remove(verificationCode);
            await context.SaveChangesAsync();
        }
    }
}