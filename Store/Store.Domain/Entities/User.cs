using Microsoft.AspNetCore.Identity;

namespace Store.Domain.Entities;

public class User : IdentityUser<Guid>
{
    public string Role { get; set; }
}