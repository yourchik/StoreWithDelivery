using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Store.Application.DTOs;
using Store.Application.Services.Interfaces.Entities;
using Store.Domain.Entities;

namespace Store.Application.Services.Implementations.Entities;

public class AuthService : IAuthService
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private const string AuthScheme = "Cookies";
    private const string InvalidLoginMessage = "Invalid login or password";

    public AuthService(UserManager<User> userManager, SignInManager<User> signInManager, IHttpContextAccessor httpContextAccessor)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<(bool Succeeded, string[]? Errors)> RegisterAsync(RegisterDto registerDto)
    {
        var user = new User
        {
            UserName = registerDto.UserName,
            Role = registerDto.Role
        };
        var result = await _userManager.CreateAsync(user, registerDto.Password);
        
        if (!result.Succeeded)
            return (false, result.Errors.Select(e => e.Description).ToArray());
        
        return (true, null);
    }

    public async Task<(bool Succeeded, string? Message)> LoginAsync(LoginDto loginDto)
    {
        var result = await _signInManager.PasswordSignInAsync(loginDto.UserName, loginDto.Password, false, false);
        if(!result.Succeeded)
            return (false, InvalidLoginMessage);

        var user = await _userManager.FindByNameAsync(loginDto.UserName);
        
        if (user?.UserName == null) 
            return (true, null);
        
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(ClaimTypes.Role, user.Role)
        };

        var identity = new ClaimsIdentity(claims, AuthScheme);
        var principal = new ClaimsPrincipal(identity);
        await _httpContextAccessor.HttpContext.SignInAsync(AuthScheme, principal);

        return (true, null);
    }

    public Task LogoutAsync()
    {
        return _httpContextAccessor.HttpContext.SignOutAsync(AuthScheme);
    }
}