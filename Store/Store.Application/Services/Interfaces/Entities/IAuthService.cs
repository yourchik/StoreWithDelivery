using Store.Application.Dtos.Auth;
using Store.Application.Services.Interfaces.Results;

namespace Store.Application.Services.Interfaces.Entities;

public interface IAuthService
{
    Task<IResult> RegisterAsync(RegisterUserDto registerUserDto);
    Task<IResult> LoginAsync(LoginUserDto loginUserDto);
    Task<IResult> LogoutAsync();
}