using Store.Application.DTOs;

namespace Store.Application.Services.Interfaces.Entities;

public interface IAuthService
{
    Task<(bool Succeeded, string[]? Errors)> RegisterAsync(RegisterDto registerDto);
    Task<(bool Succeeded, string? Message)> LoginAsync(LoginDto loginDto);
    Task LogoutAsync();
}