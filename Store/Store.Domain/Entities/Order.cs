using Contracts.Enum;

namespace Store.Domain.Entities;

public class Order : BaseEntity
{
    public required string Address { get; set; }
    public required User User { get; set; }
    public Guid UserId { get; set; }
    public required IEnumerable<Product> Products { get; set; }
    public OrderStatus Status { get; set; } = OrderStatus.Created;
}

