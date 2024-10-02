using Store.Application.ModelsDto.Auth;
using Store.Application.Services.Implementations.Results;
using Store.Application.Services.Interfaces.Results;

namespace Store.Application.Services.Interfaces.Entities;

public interface IAuthenticationService
{
    Task<IResult> RegisterAsync(RegisterUserDto registerUserDto);
    Task<EntityResult<string>> LoginAsync(LoginUserDto loginUserDto);
    Task<IResult> LogoutAsync();
}