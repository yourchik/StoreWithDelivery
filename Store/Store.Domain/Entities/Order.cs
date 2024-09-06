namespace Store.Domain.Entities;

public class Order : BaseEntity
{
    public string Address { get; set; }
    public User User { get; set; }
    public IEnumerable<Product> Products { get; set; }
    public OrderStatus Status { get; set; } = OrderStatus.Created;
}

