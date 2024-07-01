namespace Store.Domain.Entities;

public class Order : BaseEntity
{
    public string Address { get; set; }
    public List<Product> Products { get; set; }
    public OrderStatus Status { get; set; }
}

