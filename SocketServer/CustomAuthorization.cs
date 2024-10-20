using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using SocketServer.Interfaces;
using SocketServer.Services;

namespace SocketServer;

public class CustomAuthorization(DeviceService deviceService) : AuthorizationHandler<CustomAuthorizationRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, CustomAuthorizationRequirement requirement)
    {
        var httpContext = context.Resource as HttpContext;

        if (httpContext == null) 
        {
            context.Fail();
            return Task.CompletedTask;
        }
        
        var token = httpContext.Request.Headers.Authorization.ToString().Replace("Bearer ", "");
        
        if (string.IsNullOrEmpty(token))
        {
            context.Fail();
            return Task.CompletedTask;
        }
        
        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token);
        
        var idDeviceClaim = jwtToken.Claims.FirstOrDefault(claim => claim.Type == "idDevice");
        
        if (idDeviceClaim == null)
        {
            context.Fail();
            return Task.CompletedTask;
        }
        
        var tokenDb = deviceService.GetDeviceToken(int.Parse(idDeviceClaim.Value));
        
        if (tokenDb == null || tokenDb.Token != token)
        {
            context.Fail();
            return Task.CompletedTask;
        }
        
        var identity = new ClaimsIdentity(new []
        {
            new Claim("IdDevice", idDeviceClaim.Value)
        });
        context.User.AddIdentity(identity);
        context.Succeed(requirement);
        return Task.CompletedTask;
    }
}
