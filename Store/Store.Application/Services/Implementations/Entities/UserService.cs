using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Store.Application.Services.Interfaces.Entities;
using Store.Domain.Entities;

namespace Store.Application.Services.Implementations.Entities;

public class UserService(IHttpContextAccessor httpContextAccessor, UserManager<User> userManager) : IUserService
{
    public async Task<User> GetCurrentUserAsync()
    {
        if (httpContextAccessor.HttpContext?.User == null)
            throw new InvalidOperationException("HttpContext or User is not available.");

        var userClaims = httpContextAccessor.HttpContext.User;
        var user = await userManager.GetUserAsync(userClaims);
        
        if (user == null)
            throw new InvalidOperationException("User not found.");

        return user;
    }
    
    public IEnumerable<string> GetUserRoles()
    {
        if (httpContextAccessor.HttpContext?.User == null)
            throw new InvalidOperationException("HttpContext or User is not available.");
        var roles = httpContextAccessor.HttpContext.User.FindAll(ClaimTypes.Role).Select(role => role.Value).ToList();

        if (roles.Count != 0)
            return Array.Empty<string>();
        
        return roles;
    }
}