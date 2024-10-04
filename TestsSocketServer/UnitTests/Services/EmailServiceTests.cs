using System.Text;
using RestSharp;
using SocketServer.Services;

namespace TestsSocketServer.UnitTests.Services;

[TestFixture]
public class EmailServiceTests
{
    private EmailService _emailService;
    private RestClient _mailgunClient;
    private string _apiKey;
    private string _domain;

    private RestRequest CreateMailGunRequest(string to, string subject, string text)
    {
        var request = new RestRequest($"{_domain}/messages");
        
        request.AddHeader("Authorization", $"Basic {Convert.ToBase64String(Encoding.UTF8.GetBytes($"api:{_apiKey}"))}");
        request.AddParameter("from", "Mobilelink <mobilelink2025@gmail.com>");
        request.AddParameter("to", to);
        request.AddParameter("subject", subject);
        request.AddParameter("text", text);
        request.Method = Method.Post;
        
        return request;
    }

    [SetUp]
    public void Setup()
    {
        _apiKey = Environment.GetEnvironmentVariable("MAILGUN_API_KEY") ?? "ddab23b395b2d0edd188f869bf22f39f-826eddfb-ad8f9f39";
        _domain = Environment.GetEnvironmentVariable("MAILGUN_DOMAIN") ?? "sandboxe6c1ab41d5a545c9b4c86965dcb9bc28.mailgun.org";

        _mailgunClient = new RestClient($"https://api.mailgun.net/v3");
        _emailService = new EmailService(_apiKey, _domain);
    }

    [Test]
    public async Task SendVerificationEmailAsync()
    {
        const string email = "arcanghost16@gmail.com";
        const string verificationCode = "123456";

        await _emailService.SendVerificationEmailAsync(email, verificationCode);

        var request = CreateMailGunRequest(email, "Mobilelink - Código de Verificação", $"Seu código de verificação é: {verificationCode}");
        var response = await _mailgunClient.ExecuteAsync(request);
        
        Assert.That(response.Content, Does.Contain("message").IgnoreCase);
    }

    [Test]
    public void SendVerificationEmailAsync_InvalidEmail()
    {
        const string email = "arcanghost16";
        const string verificationCode = "123456";

        Assert.ThrowsAsync<Exception>(async () =>
            await _emailService.SendVerificationEmailAsync(email, verificationCode));
    }
    
    [Test]
    public void SendVerificationEmailAsync_InvalidApiKey()
    {
        const string email = "arcanghost16@gmail.com";
        const string verificationCode = "123456";
        
        var emailService = new EmailService("invalidApiKey", _domain);
        
        Assert.ThrowsAsync<Exception>(async () => 
            await emailService.SendVerificationEmailAsync(email, verificationCode));
    }

    [Test]
    public void SendVerificationEmailAsync_InvalidDomain()
    {
        const string email = "arcanghost16@gmail.com";
        const string verificationCode = "123456";
        
        var emailService = new EmailService(_apiKey, "invalidDomain");
        
        Assert.ThrowsAsync<Exception>(async () => 
            await emailService.SendVerificationEmailAsync(email, verificationCode));
    }
    
    [TearDown]
    public void TearDown()
    {
        _mailgunClient.Dispose();
    }
}
