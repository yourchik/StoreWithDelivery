namespace Store.Domain.Entities;

public class Basket : BaseEntity
{
    public required User User { get; set; }
    
    public Guid UserId { get; set; }
    
    public ICollection<Product> Products { get; set; } = new List<Product>();
}