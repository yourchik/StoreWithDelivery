using Store.Domain.Entities;

namespace Store.Application.Services.Interfaces.Entities;

public interface IUserService
{
    Task<User> GetCurrentUserAsync();
    IEnumerable<string> GetUserRoles();
}