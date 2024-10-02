namespace Store.Application.ModelsDto.Auth;

public class UserDto
{
    public required string UserName { get; init; }
    public required string Password { get; init; }
}