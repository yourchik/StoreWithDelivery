using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Store.Application.Dtos.AuthDtos;
using Store.Application.Services.Adapters;
using Store.Application.Services.Factories;
using Store.Application.Services.Implementations.Results;
using Store.Application.Services.Interfaces.Entities;
using Store.Application.Services.Interfaces.Results;
using Store.Application.Settings;
using Store.Domain.Entities;

namespace Store.Application.Services.Implementations.Entities;

public class AuthService : IAuthService
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private const string InvalidLoginMessage = "Invalid login or password";
    private readonly string[] _errors = new[] { "User not found." };
    private readonly JwtSettings _jwtSettings;

    public AuthService(UserManager<User> userManager, 
        SignInManager<User> signInManager,
        IOptions<JwtSettings> jwtSettings)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _jwtSettings = jwtSettings.Value;
    }

    public async Task<IResult> RegisterAsync(RegisterUserDto registerUserDto)
    {
        var user = new User
        {
            UserName = registerUserDto.UserName,
        };
        var result = await _userManager.CreateAsync(user, registerUserDto.Password);
        var adapter = new IdentityResultAdapter(result);
        
        var role = registerUserDto.Role.ToString();
        var roleResult = await _userManager.AddToRoleAsync(user, role);
        
        if (!roleResult.Succeeded)
            return ResultFactory.CreateResult(false, roleResult.Errors.Select(e => e.Description).ToArray());
        
        return ResultFactory.CreateResult(adapter);
    }

    public async Task<EntityResult<string>> LoginAsync(LoginUserDto loginUserDto)
    {
        var result = await _signInManager.PasswordSignInAsync(loginUserDto.UserName, loginUserDto.Password, false, false);
        var adapter = new SignInResultAdapter(result);

        if (!adapter.IsSuccess)
            return EntityResult<string>.Failure(adapter.Errors.ToArray());

        var user = await _userManager.FindByNameAsync(loginUserDto.UserName);
            
        if (user?.UserName == null) 
            return EntityResult<string>.Failure(_errors);
            
        var token = await GenerateJwtToken(user);
        return EntityResult<string>.Success(token);
    }
    
    public Task<IResult> LogoutAsync()
    {
        return Task.FromResult(ResultFactory.CreateResult(true));
    }
    
    private async Task<string> GenerateJwtToken(User user)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.UserName!),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
        };
        var userRoles = await _userManager.GetRolesAsync(user);
        claims.AddRange(userRoles.Select(role => new Claim(ClaimTypes.Role, role)));
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.Now.AddMinutes(30),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}