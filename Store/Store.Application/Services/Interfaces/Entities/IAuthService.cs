using Store.Application.Dtos.AuthDtos;
using Store.Application.Services.Implementations.Results;
using Store.Application.Services.Interfaces.Results;

namespace Store.Application.Services.Interfaces.Entities;

public interface IAuthService
{
    Task<IResult> RegisterAsync(RegisterUserDto registerUserDto);
    Task<EntityResult<string>> LoginAsync(LoginUserDto loginUserDto);
    Task<IResult> LogoutAsync();
}