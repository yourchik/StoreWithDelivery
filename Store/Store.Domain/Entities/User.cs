using Microsoft.AspNetCore.Identity;

namespace Store.Domain.Entities;

public class User : IdentityUser<Guid>
{
    public IEnumerable<Order> Orders { get; set; }
}