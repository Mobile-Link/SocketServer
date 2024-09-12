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
        var client = new RestClient($"https://api.mailgun.net/v3");
        
        var authHeader = Convert.ToBase64String(Encoding.UTF8.GetBytes($"api:{_apiKey}"));
        Console.WriteLine($"API Key codificada: {authHeader}");
        client.AddDefaultHeader("Authorization", $"Basic {authHeader}");
        
        var request = new RestRequest();
        
        request.AddParameter("domain", _domain, ParameterType.UrlSegment);
        request.Resource = "{domain}/messages";
        request.AddParameter("from", $"Mobilelink <mobilelink2025@gmail.com>");
        request.AddParameter("to", email);
        request.AddParameter("subject", "Código de verificação");
        request.AddParameter("text", $"Seu código de verificação é: {verificationCode}");
        request.Method = Method.Post;
        
        var response = await client.ExecuteAsync(request);
        
        Console.WriteLine(response.Content);
        Console.WriteLine(response.StatusCode);

        if (response.StatusCode != HttpStatusCode.OK)
        {
            throw new Exception("Erro ao enviar email");
        }
    }
}    