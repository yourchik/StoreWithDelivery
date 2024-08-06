using Store.Domain.Enums;

namespace Store.Application.Dtos.Auth;

public class RegisterUserDto : UserDto
{
    public UserRole Role { get; set; }
}