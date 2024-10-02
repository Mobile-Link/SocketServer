using System.Net;
using System.Text;
using RestSharp;
using SocketServer.Services;

namespace TestsSocketServer.UnitTests.Services;

[TestFixture]
public class EmailServiceTests
{
    private EmailService _emailService;
    private string? _apiKey;
    private string? _domain;

    [SetUp]
    public void Setup()
    {
        _apiKey = Environment.GetEnvironmentVariable("MAILGUN_API_KEY")!;
        _domain = Environment.GetEnvironmentVariable("MAILGUN_DOMAIN")!;

        if (string.IsNullOrEmpty(_domain) || string.IsNullOrEmpty(_apiKey))
        {
            _domain = "sandboxe6c1ab41d5a545c9b4c86965dcb9bc28.mailgun.org";
            _apiKey = "ddab23b395b2d0edd188f869bf22f39f-826eddfb-ad8f9f39";
        }

        _emailService = new EmailService(_apiKey, _domain);
    }

    [Test]
    public async Task SendVerificationEmailAsync()
    {
        const string email = "arcanghost16@gmail.com";
        const string verificationCode = "123456";

        await _emailService.SendVerificationEmailAsync(email, verificationCode);

        var client = new RestClient($"https://api.mailgun.net/v3");
        var request = new RestRequest($"{_domain}/messages");

        var authHeader = Convert.ToBase64String(Encoding.UTF8.GetBytes($"api:{_apiKey}"));
        request.AddHeader("Authorization", $"Basic {authHeader}");

        request.AddParameter("from", $"Mobilelink <mobilelink2025@gmail.com>");
        request.AddParameter("to", email);
        request.AddParameter("subject", "Código de verificação");
        request.AddParameter("text", $"Seu código de verificação é: {verificationCode}");
        request.Method = Method.Post;

        var response = await client.ExecuteAsync(request);
        Assert.Multiple(() =>
        {
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            Assert.That(response.Content, Does.Contain("message").IgnoreCase);
        });
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
        
        Assert.ThrowsAsync<Exception>(async () => await emailService.SendVerificationEmailAsync(email, verificationCode));
    }

    [Test]
    public void SendVerificationEmailAsync_InvalidDomain()
    {
        const string email = "arcanghost16@gmail.com";
        const string verificationCode = "123456";
        
        var emailService = new EmailService(_apiKey, "invalidDomain");
        
        Assert.ThrowsAsync<Exception>(async () => await emailService.SendVerificationEmailAsync(email, verificationCode));
    }
}
