using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using MongoDB.Driver;
using SocketServer.ChatHub;
using SocketServer.Data;
using SocketServer.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
builder.Services.AddSignalR();
builder.Services.AddScoped<TransferService>();
builder.Services.AddScoped<ConnectionManagerService>();
builder.Services.AddScoped<ConnectionManagerService>(sp =>
    new ConnectionManagerService(sp.GetRequiredService<IHubContext<TransferHub>>()));
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<DeviceService>();
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

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Authenticated", policy => policy.RequireAuthenticatedUser());
});

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(
        builder.Configuration.GetConnectionString("DefaultConnection")
    ));

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

app.MapHub<TransferHub>("/transferhub");

app.MapControllers();

app.Run();