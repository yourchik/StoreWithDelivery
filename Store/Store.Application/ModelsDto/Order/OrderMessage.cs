using Store.Domain.Entities;

namespace Store.Application.ModelsDto.Order;

public class OrderMessage
{
    public Guid OrderId { get; set; }
    public string Address { get; set; }
    public IEnumerable<Domain.Entities.Product> Products { get; set; }
    public OrderStatus Status { get; set; }
}