using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Store.Application.Dtos.Auth;
using Store.Application.Services.Adapters;
using Store.Application.Services.Factories;
using Store.Application.Services.Interfaces.Entities;
using Store.Application.Services.Interfaces.Results;
using Store.Domain.Entities;

namespace Store.Application.Services.Implementations.Entities;

public class AuthService : IAuthService
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private const string AuthScheme = "Cookies";
    private const string InvalidLoginMessage = "Invalid login or password";
    private readonly string[] _errors = new[] { "User not found." };

    public AuthService(UserManager<User> userManager, 
        SignInManager<User> signInManager,
        IHttpContextAccessor httpContextAccessor)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<IResult> RegisterAsync(RegisterUserDto registerUserDto)
    {
        var user = new User
        {
            UserName = registerUserDto.UserName,
            Role = registerUserDto.Role
        };
        var result = await _userManager.CreateAsync(user, registerUserDto.Password);
        var adapter = new IdentityResultAdapter(result);
            
        return ResultFactory.CreateResult(adapter);
    }

    public async Task<IResult> LoginAsync(LoginUserDto loginUserDto)
    {
        var result = await _signInManager.PasswordSignInAsync(loginUserDto.UserName, loginUserDto.Password, false, false);
        var adapter = new SignInResultAdapter(result);
        
        if (!adapter.IsSuccess)
            return ResultFactory.CreateResult(adapter);

        var user = await _userManager.FindByNameAsync(loginUserDto.UserName);
        
        if (user?.UserName == null) 
            return ResultFactory.CreateResult(false, _errors);
        
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(ClaimTypes.Role, user.Role)
        };

        var identity = new ClaimsIdentity(claims, AuthScheme);
        var principal = new ClaimsPrincipal(identity);
        await _httpContextAccessor.HttpContext.SignInAsync(AuthScheme, principal);

        return ResultFactory.CreateResult(true);
    }

        public Task<IResult> LogoutAsync()
        {
            return _httpContextAccessor.HttpContext.SignOutAsync(AuthScheme).ContinueWith(task => 
                ResultFactory.CreateResult(task.IsCompletedSuccessfully)
            );
        }
}