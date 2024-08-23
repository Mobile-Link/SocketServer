using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using SocketServer.ChatHub;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSignalR();

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.MapHub<ChatHub>("/chathub");

app.Run();