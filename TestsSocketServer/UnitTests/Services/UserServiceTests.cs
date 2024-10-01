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

    private void AssertBadRequestResult(IActionResult result, string errorMessage)
    {
        Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
        var badRequestResult = result as BadRequestObjectResult;
        Assert.That(badRequestResult.Value, Is.Not.Null);
        Assert.That(badRequestResult.Value.GetType(), Is.EqualTo(typeof(Dictionary<string, string>)));
        Assert.That(((Dictionary<string, string>)badRequestResult.Value)["error"], Is.EqualTo(errorMessage));
    }

    private void AssertNotFoundResult(IActionResult result, string errorMessage)
    {
        Assert.That(result, Is.InstanceOf<NotFoundObjectResult>());
        var notFoundResult = result as NotFoundObjectResult;
        Assert.That(notFoundResult.Value, Is.Not.Null);
        Assert.That(notFoundResult.Value.GetType(), Is.EqualTo(typeof(Dictionary<string, string>)));
        Assert.That(((Dictionary<string, string>)notFoundResult.Value)["error"], Is.EqualTo(errorMessage));
    }

    private void AssertOkObjectResult(IActionResult result, string successMessage)
    {
        Assert.That(result, Is.InstanceOf<OkObjectResult>());
        var okResult = result as OkObjectResult;
        Assert.That(okResult.Value, Is.Not.Null);
        var message = okResult.Value as Dictionary<string, string>;
        Assert.That(message["message"], Is.EqualTo(successMessage));
    }

    private void AssertUser(User result, string email, string username)
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

        Assert.That(result, Is.InstanceOf<OkObjectResult>());
        var okResult = result as OkObjectResult;
        Assert.That(okResult.Value, Is.Not.Null);
        var createdUser = okResult.Value as User;
        Assert.That(createdUser, Is.Not.Null);

        Assert.Multiple(() =>
        {
            Assert.That(createdUser.Email, Is.EqualTo(expectedUser.Email));
            Assert.That(createdUser.Username, Is.EqualTo(expectedUser.Username));
            Assert.That(BCrypt.Net.BCrypt.Verify(expectedUser.PasswordHash, createdUser.PasswordHash), Is.True); 
        });

        Assert.That(await _context.Users.CountAsync(), Is.EqualTo(1));
    }

    private Register CreateRegisterRequest(string email, string username, string password)
    {
        return new Register
        {
            Email = email,
            Username = username,
            Password = password
        };
    }
    private UpdateUser CreateUpdateUserRequest(string username = null, string email = null)
    {
        return new UpdateUser
        {
            Username = username,
            Email = email
        };
    }
    private UpdatePassword CreateUpdatePasswordRequest(string password)
    {
        return new UpdatePassword
        {
            Password = password
        };
    }

    [SetUp]
    public async Task SetUp()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlite("Data Source=MobileLink.db")
            .Options;

        _context = new AppDbContext(options);
        await _context.Database.MigrateAsync();

        _userService = new UserService(_context);
    }

    [Test]
    public async Task Register_ValidRequest_ShouldCreateUser()
    {
        var request = CreateRegisterRequest("emailDeTeste@gmail.com", "UserTeste", "123456"); 

        var expectedUser = new User
        {
            Email = "emailDeTeste@gmail.com",
            Username = "UserTeste",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("123456")
        };

        await AssertRegisterSuccess(request, expectedUser); 
    }

    [Test]
    public async Task Register_InvalidEmail_ShouldReturnBadRequest()
    {
        var request = CreateRegisterRequest("emailDeTeste", "UserTeste", "123456"); 

        var result = await _userService.Register(request);

        AssertBadRequestResult(result, "Formato de email inválido");
    }

    [Test]
    public async Task Register_EmptyEmail_ShouldReturnBadRequest()
    {
        var request = CreateRegisterRequest("", "UserTeste", "123456"); 

        var result = await _userService.Register(request);

        AssertBadRequestResult(result, "Campos são obrigatórios");
    }

    [Test]
    public async Task Register_EmptyPassword_ShouldReturnBadRequest()
    {
        var request = CreateRegisterRequest("emailDeTeste@gmail.com", "UserTeste", "");

        var result = await _userService.Register(request);

        AssertBadRequestResult(result, "Campos são obrigatórios");
    }

    [Test]
    public async Task Register_ExistingEmail_ShouldReturnBadRequest()
    {
        await CreateUser("emailDeTeste@gmail.com", "UserTeste", "123456");

        var request = CreateRegisterRequest("emailDeTeste@gmail.com", "UserTeste", "123456");

        var result = await _userService.Register(request);

        AssertBadRequestResult(result, "Email ou Username já cadastrado");
    }

    [Test]
    public async Task Register_ExistingUsername_ShouldReturnBadRequest()
    {
        await CreateUser("emailDeTeste@gmail.com", "UserTeste", "123456");

        var request = CreateRegisterRequest("emailDeTesteNovo@gmail.com", "UserTeste", "123456");

        var result = await _userService.Register(request);

        AssertBadRequestResult(result, "Email ou Username já cadastrado"); 
    }

    [Test]
    public async Task DeleteUser_ExistingUser_ShouldReturnOkObjectResult()
    {
        var user = await CreateUser("emailDeTeste@gmail.com", "UserTeste", "123456");

        var result = await _userService.DeleteUser(user.IdUser);

        AssertOkObjectResult(result, "Usuário removido com sucesso");
    }

    [Test]
    public async Task DeleteUser_NonExistingUser_ShouldReturnNotFoundObjectResult()
    {
        var result = await _userService.DeleteUser(1);

        AssertNotFoundResult(result, "Usuário não encontrado");
    }

    [Test]
    public async Task UpdateUser_ValidUsername_ShouldReturnOkObjectResult()
    {
        var user = await CreateUser("emailDeTeste@gmail.com", "UserTeste", "123456");

        var request = CreateUpdateUserRequest(username: "UserTesteNovo"); 

        var result = await _userService.UpdateUser(user.IdUser, request);

        AssertOkObjectResult(result, "Usuário atualizado com sucesso");
    }

    [Test]
    public async Task UpdatePassword_NonExistingUser_ShouldReturnNotFoundObjectResult()
    {
        var request = CreateUpdatePasswordRequest("123456");

        var result = await _userService.UpdatePassword(1, request);

        AssertNotFoundResult(result, "Usuário não encontrado");
    }

    [Test]
    public async Task UpdatePassword_ExistingUser_ShouldReturnOkObjectResult()
    {
        var user = await CreateUser("emailDeTeste@gmail.com", "UserTeste", "123456");

        var request = CreateUpdatePasswordRequest("654321");

        var result = await _userService.UpdatePassword(user.IdUser, request);

        AssertOkObjectResult(result, "Senha atualizada com sucesso");
    }

    [Test]
    public async Task GetUserByEmail_ExistingUser_ShouldReturnUser()
    {
        var user = await CreateUser("emailDeTeste@gmail.com", "UserTeste", "123456");

        var result = await _userService.GetUserByEmail(user.Email);

        AssertUser(result, "emailDeTeste@gmail.com", "UserTeste");
    }

    [Test]
    public async Task GetUserByEmail_NonExistingUser_ShouldReturnNull()
    {
        var result = await _userService.GetUserByEmail("emailDeTesteInexistente@gmail.com");

        Assert.That(result, Is.Null);
    }

    [Test]
    public async Task GetUserByUsername_ExistingUser_ShouldReturnUser()
    {
        var user = await CreateUser("emailDeTeste@gmail.com", "UserTeste", "123456");

        var result = await _userService.GetUserByUsername(user.Username);

        AssertUser(result, "emailDeTeste@gmail.com", "UserTeste");
    }

    [Test]
    public async Task GetUserByUsername_NonExistingUser_ShouldReturnNull()
    {
        var result = await _userService.GetUserByUsername("UserTesteInexistente");

        Assert.That(result, Is.Null);
    }

    [Test]
    public async Task GetUsers_ShouldReturnUsers()
    {
        await CreateUser("emailDeTeste1@gmail.com", "UserTeste1", "123456");
        await CreateUser("emailDeTeste2@gmail.com", "UserTeste2", "123456");

        var result = await _userService.GetUsers();

        Assert.That(result, Is.Not.Empty);
        Assert.That(result, Has.Count.EqualTo(2));
    }

    [Test]
    public async Task StoreVerificationCode_ShouldStoreCode()
    {
        const string email = "emailDeTeste@gmail.com";
        const string code = "123456";

        await _userService.StoreVerificationCode(email, code);

        var storedCode = await _context.VerificationCodes.FirstOrDefaultAsync(vc => vc.Email == email);

        Assert.That(storedCode, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(storedCode.Email, Is.EqualTo(email));
            Assert.That(storedCode.Code, Is.EqualTo(code));
        });
    }

    [Test]
    public async Task GetVerificationCode_ExistingCode_ShouldReturnCode()
    {
        const string email = "emailDeTeste@gmail.com";
        const string code = "123456";

        await _userService.StoreVerificationCode(email, code);

        var result = await _userService.GetVerificationCode(email);

        Assert.That(result, Is.EqualTo(code));
    }

    [Test]
    public async Task GetVerificationCode_ExpiredCode_ShouldReturnNull()
    {
        const string email = "emailDeTeste@gmail.com";
        const string code = "123456";

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
    public async Task DeleteVerificationCode_ShouldDeleteCode()
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