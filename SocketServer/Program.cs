using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Hosting;
using SocketServer.ChatHub;
using SocketServer.FileTransferHub;
using SocketServer.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSignalR();
builder.Services.AddScoped<FileTransferService>();
builder.Services.AddScoped<ConnectionManagerService>();

builder.Services.AddScoped<ConnectionManagerService>(sp =>
    new ConnectionManagerService(sp.GetRequiredService<IHubContext<FileTransferHub>>()));

var app = builder.Build();

app.MapGet("/", () => "Hello World!");
app.MapGet("/filetransferhub", () => "File Transfer Hub");

app.MapHub<ChatHub>("/chathub");


app.Run();