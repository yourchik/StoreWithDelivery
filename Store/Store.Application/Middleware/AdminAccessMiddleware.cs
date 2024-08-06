using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Store.Domain.Enums;

namespace Store.Application.Middleware;

public class AdminAccessMiddleware
{
    private readonly RequestDelegate _next;

    public AdminAccessMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var user = context.User;
        
        if (user.Identity is { IsAuthenticated: true } && user.IsInRole(nameof(UserRole.Admin)))
        {
            var claims = Enum.GetValues<UserRole>().Select(role => new Claim(ClaimTypes.Role, role.ToString())).ToList();
            var identity = new ClaimsIdentity(claims, nameof(UserRole.Admin));
            var principal = new ClaimsPrincipal(identity);
            context.User = principal;
        }

        await _next(context);
    }
}
