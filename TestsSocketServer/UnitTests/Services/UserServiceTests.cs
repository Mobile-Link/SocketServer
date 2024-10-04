using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SocketServer.Data;
using SocketServer.Entities;
using SocketServer.Models;
using SocketServer.Services;

namespace TestsSocketServer.UnitTests.Services;

[TestFixture]
public class UserServiceTests
{
    private UserService _userService;
    private AppDbContext _context;

    [SetUp]
    public Task SetUp()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase("MobileLink")
            .Options;

        _context = new AppDbContext(options);

        _userService = new UserService(_context);
        return Task.CompletedTask;
    }
    
    private async Task<User> CreateUser(string email, string username, string password)
    {
        var user = new User
        {
            Email = email,
            Username = username,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(password)
        };

        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();

        return user;
    }

    private static void AssertActionResult<T>(IActionResult result, string expectedMessage, string key = "message")
        where T : ObjectResult
    {
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.Not.Null, "resultado deveria ser diferente de nulo");
            Assert.That(result, Is.InstanceOf<T>(), $"resultado deveria ser do tipo {typeof(T)}");
        
            var objectResult = result as T;
            Assert.That(objectResult?.Value, Is.Not.Null, "valor do resultado deveria ser diferente de nulo");
        
            var message = objectResult?.Value as Dictionary<string, string>;
            Assert.That(message?[key], Is.EqualTo(expectedMessage), $"mensagem deveria ser igual a {expectedMessage}");
        });
    }

    private static void AssertUser(User result, string email, string username)
    {
        Assert.That(result, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(result.Email, Is.EqualTo(email));
            Assert.That(result.Username, Is.EqualTo(username));
        });
    }

    private async Task AssertRegisterSuccess(Register request, User expectedUser)
    {
        var result = await _userService.Register(request);

        Assert.That(result, Is.AssignableFrom<OkObjectResult>());
        
        var okResult = result as OkObjectResult;
        Assert.That(okResult?.Value, Is.Not.Null);

        var createdUser = okResult?.Value as User;

        Assert.Multiple(() =>
        {
            Assert.That(createdUser, Is.Not.Null);
            Assert.That(createdUser?.Email, Is.EqualTo(expectedUser.Email));
            Assert.That(createdUser?.Username, Is.EqualTo(expectedUser.Username));
            Assert.That(BCrypt.Net.BCrypt.Verify(request.Password, createdUser?.PasswordHash), Is.True); 
        });

        Assert.That(await _context.Users.CountAsync(), Is.EqualTo(1));
    }
    
    private static Register CreateRegister(string email, string username, string password)
    {
        return new Register
        {
            Email = email,
            Username = username,
            Password = password
        };
    }
    
    private static UpdateUser CreateUpdateUser(string username, string email)
    {
        return new UpdateUser
        {
            Username = username,
            Email = email
        };
    }
    
    private static UpdatePassword CreateUpdatePassword(string password)
    {
        return new UpdatePassword
        {
            Password = password
        };
    }

    [Test]
    public async Task Register_ValidRequest_CreateUser()
    {
        var request = CreateRegister("emailDeTeste@gmail.com", "UserTeste", "123456"); 

        var expectedUser = new User
        {
            Email = "emailDeTeste@gmail.com",
            Username = "UserTeste",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password)
        };

        await AssertRegisterSuccess(request, expectedUser); 
    }

    [TestCase("emailDeTeste", "UserTeste", "123456", "Formato de email inválido", "error")]
    [TestCase("", "UserTeste", "123456", "Campos são obrigatórios", "error")]
    [TestCase("emailDeTeste@gmail.com", "UserTeste", "", "Campos são obrigatórios", "error")]
    public async Task Register_InvalidInputs(string email, string username, string password, string expectedMessage, string key)
    {
        var request = CreateRegister(email, username, password);

        var result = await _userService.Register(request);

        AssertActionResult<BadRequestObjectResult>(result, expectedMessage, key);
    }

    [TestCase("emailDeTeste@gmail.com", "UserTesteNovo", "Email ou Username já cadastrado")]
    [TestCase("emailDeTesteNovo@gmail.com", "UserTeste", "Email ou Username já cadastrado")]
    public async Task Register_ExistingEmailOrUsername(string email, string username, string expectedMessage)
    {
        await CreateUser("emailDeTeste@gmail.com", "UserTeste", "123456");
        
        var request = CreateRegister(email, username, "123456");
        
        var result = await _userService.Register(request);
        
        AssertActionResult<BadRequestObjectResult>(result, expectedMessage, "error");
    }

    [TestCase(true, "Usuário removido com sucesso", typeof(OkObjectResult))]
    [TestCase(false, "Usuário não encontrado", typeof(NotFoundObjectResult))]
    public async Task DeleteUser(bool userExists, string expectedMessage, Type expectedType)
    {
        if (userExists)
        {
            var user = await CreateUser("emailDeTeste@gmail.com", "UserTeste", "123456");
            var result = await _userService.DeleteUser(user.IdUser);
            AssertActionResult<OkObjectResult>(result, expectedMessage);
        }
        else
        {
            var result = await _userService.DeleteUser(1);
            AssertActionResult<NotFoundObjectResult>(result, expectedMessage, "error");
        }
    }

    [TestCase(true, "UserTesteNovo", "emailDeTeste@gmail.com", "Usuário atualizado com sucesso", typeof(OkObjectResult))]
    [TestCase(false, "UserTeste", "emailDeTeste@gmail.com", "Usuário não encontrado", typeof(NotFoundObjectResult))]
    public async Task UpdateUser(bool userExists, string username, string email, string expectedMessage, Type expectedType)
    {
        if (userExists)
        {
            var user = await CreateUser("emailDeTeste@gmail.com", "UserTeste", "123456");
            var request = CreateUpdateUser(username, email);
            var result = await _userService.UpdateUser(user.IdUser, request);
            
            AssertActionResult<OkObjectResult>(result, expectedMessage);
        }
        else
        {
            var result = await _userService.UpdateUser(1, CreateUpdateUser(username, email));
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Null, "resultado não deveria ser nulo");
                Assert.That(expectedType, Is.EqualTo(typeof(NotFoundObjectResult)), "tipo de resultado deveria ser NotFoundObjectResult");
            });
        }
    }

    [Test]
    public async Task UpdatePassword_ExistingUser()
    {
        var user = await CreateUser("emailDeTeste@gmail.com", "UserTeste", "123456");

        var request = CreateUpdatePassword("654321");

        var result = await _userService.UpdatePassword(user.IdUser, request);

        AssertActionResult<OkObjectResult>(result, "Senha atualizada com sucesso");
    }

    [Test]
    public async Task GetUserByEmail_ExistingUser()
    {
        var user = await CreateUser("emailDeTeste@gmail.com", "UserTeste", "123456");

        var result = await _userService.GetUserByEmail(user.Email);

        AssertUser(result, "emailDeTeste@gmail.com", "UserTeste");
    }

    [Test]
    public async Task GetUserByEmail_NonExistingUser()
    {
        var result = await _userService.GetUserByEmail("emailDeTesteInexistente@gmail.com");

        Assert.That(result, Is.Null);
    }

    [Test]
    public async Task GetUserByUsername_ExistingUser()
    {
        var user = await CreateUser("emailDeTeste@gmail.com", "UserTeste", "123456");

        var result = await _userService.GetUserByUsername(user.Username);

        AssertUser(result, "emailDeTeste@gmail.com", "UserTeste");
    }

    [Test]
    public async Task GetUserByUsername_NonExistingUser()
    {
        var result = await _userService.GetUserByUsername("UserTesteInexistente");

        Assert.That(result, Is.Null);
    }

    [Test]
    public async Task GetUsers()
    {
        await CreateUser("emailDeTeste1@gmail.com", "UserTeste1", "123456");
        await CreateUser("emailDeTeste2@gmail.com", "UserTeste2", "123456");

        var result = await _userService.GetUsers();

        Assert.That(result, Is.Not.Empty);
        Assert.That(result, Has.Count.EqualTo(2));
    }

    [Test]
    public async Task StoreVerificationCode()
    {
        const string email = "emailDeTeste@gmail.com";
        const string code = "123456";

        await _userService.StoreVerificationCode(email, code);

        var storedCode = await _context.VerificationCodes.FirstOrDefaultAsync(vc => vc.Email == email);

        Assert.That(storedCode, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(storedCode?.Email, Is.EqualTo(email));
            Assert.That(storedCode?.Code, Is.EqualTo(code));
        });
    }

    [Test]
    public async Task GetVerificationCode_ExistingCode()
    {
        const string email = "emailDeTeste@gmail.com";
        const string code = "123456";

        await _userService.StoreVerificationCode(email, code);

        var result = await _userService.GetVerificationCode(email);

        Assert.That(result, Is.EqualTo(code));
    }

    [Test]
    public async Task GetVerificationCode_ExpiredCode()
    {
        const string email = "emailDeTeste@gmail.com";
        const string code = "";

        var verificationCode = new VerificationCode
        {
            Email = email,
            Code = code,
            CreationDate = DateTime.Now,
            ExpirationDate = DateTime.Now.AddMinutes(-1)
        };

        await _context.VerificationCodes.AddAsync(verificationCode);
        await _context.SaveChangesAsync();

        var result = await _userService.GetVerificationCode(email);

        Assert.That(result, Is.Null);
    }

    [Test]
    public async Task DeleteVerificationCode()
    {
        const string email = "emailDeTeste@gmail.com";
        const string code = "123456";

        await _userService.StoreVerificationCode(email, code);

        await _userService.DeleteVerificationCode(email);

        var storedCode = await _context.VerificationCodes.FirstOrDefaultAsync(vc => vc.Email == email);

        Assert.That(storedCode, Is.Null);
    }

    [TearDown]
    public async Task TearDown()
    {
        await _context.Database.EnsureDeletedAsync();
        await _context.DisposeAsync();
    }
}
