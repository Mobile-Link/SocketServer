using System.Net;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using RestSharp;

namespace SocketServer.Services;

public class EmailService
{
    private readonly string _apiKey;
    private readonly string _domain;
    
    public EmailService(string apiKey, string domain)
    {
        _apiKey = apiKey;
        _domain = domain;
    }
    public async Task SendVerificationEmailAsync(string email, string verificationCode)
    {
        var client = new RestClient($"https://api.mailgun.net/v3/{_domain}/messages");
        var request = new RestRequest()
        {
            Method = Method.Get
        };
        
        request.AddHeader("Authorization", $"Basic {Convert.ToBase64String(Encoding.UTF8.GetBytes($"api:{_apiKey}"))}");
        request.AddParameter("from", $"Mobilelink <mobilelink2025@gmail.com>");
        request.AddParameter("to", email);
        request.AddParameter("subject", "Código de verificação");
        request.AddParameter("text", $"Seu código de verificação é: {verificationCode}");
        
        var response = await client.ExecuteAsync(request);

        if (response.StatusCode != HttpStatusCode.OK)
        {
            throw new Exception("Erro ao enviar email");
        }
    }
}    