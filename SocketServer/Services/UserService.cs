using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using SocketServer.Models;

namespace SocketServer.Services;

public class UserService()
{
    private List<User> _users = new List<User>();
    private int _nextId = 1;

    public IActionResult Register(RegisterRequest request)
    {
        if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Password))
        {
            return new BadRequestObjectResult( new{ error = "Email e senha são obrigatórios" });
        }

        if (!new EmailAddressAttribute().IsValid(request.Email))
        {
            return new BadRequestObjectResult(new {error = "Formato de email inválido"});
        }
        
        var existingUser = _users.FirstOrDefault(u => u.Email == request.Email);
        
        if (existingUser != null)
        {
            return new BadRequestObjectResult(new {error = "Email já cadastrado"});
        }
        
        var user = new User
        {
            Id = _nextId++,
            Email = request.Email,
            PasswordHash = request.Password
        };

        _users.Add(user);

        //TODO Normalizar saída da service
        return new OkObjectResult(user);
    }
    
    public List<User> GetUsers()
    {
        return _users.Skip(Math.Max(0, _users.Count - 10)).ToList();
    }
}