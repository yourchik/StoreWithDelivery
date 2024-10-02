using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Store.Domain.Enums;

namespace Store.Application.Middleware;

public class AdminAccessMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        var user = context.User;
        if (user.Identity is { IsAuthenticated: true } && user.IsInRole(nameof(UserRole.Admin)))
        {
            var claims = user.Claims.ToList();
            
            var roleClaims = Enum.GetValues<UserRole>()
                .Select(role => new Claim(ClaimTypes.Role, role.ToString()))
                .ToList();

            claims.AddRange(roleClaims);
            var identity = new ClaimsIdentity(claims, user.Identity.AuthenticationType);
            var principal = new ClaimsPrincipal(identity);
            context.User = principal;
        }

        await next(context);
    }

}
