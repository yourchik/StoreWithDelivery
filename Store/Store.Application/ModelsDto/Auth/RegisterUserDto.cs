using Store.Domain.Enums;

namespace Store.Application.ModelsDto.Auth;

public class RegisterUserDto : UserDto
{
    public UserRole Role { get; init; }
}
