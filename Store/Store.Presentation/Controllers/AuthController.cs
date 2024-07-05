using Microsoft.AspNetCore.Mvc;
using Store.Application.DTOs;
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
    public async Task<IActionResult> Register(RegisterDto registerDto)
    {
        var result = await _authService.RegisterAsync(registerDto);
        if(!result.Succeeded)
            return BadRequest(result.Errors);
        return Ok(new { Message = "User register successfully!" });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto loginDto)
    {
        var result = await _authService.LoginAsync(loginDto);
        if(!result.Succeeded)
            return Unauthorized(result.Message);
        return Ok(new { result.Message });
    }
    
    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        await _authService.LogoutAsync();
        return Ok();
    }
}