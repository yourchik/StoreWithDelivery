namespace Store.Domain.Entities;

public class User : BaseEntity
{
    public string Login { get; set; }
    public string PasswordHash { get; set; }
    public string Role { get; set; }
}