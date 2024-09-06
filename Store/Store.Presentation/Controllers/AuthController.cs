using Microsoft.AspNetCore.Mvc;
using Store.Application.Dtos.AuthDtos;
using Store.Application.Services.Interfaces.Entities;

namespace Store.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }
    
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterUserDto registerUserDto)
    {
        var result = await _authService.RegisterAsync(registerUserDto);
        if(!result.IsSuccess)
            return BadRequest(result.Errors);
        return Ok(new { Message = "User register successfully!" });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginUserDto loginUserDto)
    {
        var result = await _authService.LoginAsync(loginUserDto);
        if(!result.IsSuccess)
            return Unauthorized(result.Errors);
        return Ok(result.Value);
    }
    
    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        var result = await _authService.LogoutAsync();
        return Ok(result.IsSuccess);
    }
}