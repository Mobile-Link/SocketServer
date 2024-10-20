using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using MongoDB.Driver;
using SocketServer;
using SocketServer.Data;
using SocketServer.Hubs;
using SocketServer.Interfaces;
using SocketServer.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
builder.Services.AddSignalR();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<TransferService>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddSingleton<ConnectionService>();
builder.Services.AddScoped<DeviceService>();
builder.Services.AddScoped<HistoryService>();
builder.Services.AddScoped<EmailService>(sp => new EmailService(
    builder.Configuration["MailgunApiKey"],
    builder.Configuration["MailgunDomain"]
));
builder.Services.AddScoped<VerificationCodeService>();
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "MobileLink API",
        Version = "v1"
    });
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
        builder.Configuration.Bind("JwtSettings", options));

builder.Services.AddAuthorizationBuilder()
    .AddPolicy("Authorized", policy => policy.Requirements.Add(new CustomAuthorizationRequirement()));

builder.Services.AddScoped<IAuthorizationHandler, CustomAuthorization>();

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlite(
        builder.Configuration.GetConnectionString("DefaultConnection")
    );
    options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
});
    

builder.Services.AddDbContext<ExpirationDbContext>(options =>
    {
        options.UseMongoDB(
            builder.Configuration.GetConnectionString("MongoDBConnection"),
            builder.Configuration.GetConnectionString("MongoDBDatabaseName")
        );
    }
);

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

app.UseSwagger();
app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1"));
app.UseReDoc(c =>
{
    c.RoutePrefix = "redoc";
    c.SpecUrl = "/swagger/v1/swagger.json";
});
//TODO add auth and get idDevice from token
app.MapHub<ConnectionHub>("/connectionhub");

app.MapControllers();

app.Run();