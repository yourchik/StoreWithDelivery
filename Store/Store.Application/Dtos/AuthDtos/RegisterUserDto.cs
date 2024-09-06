using Store.Domain.Enums;

namespace Store.Application.Dtos.AuthDtos;

public class RegisterUserDto : UserDto
{
    public UserRole Role { get; set; }
}