using Store.Domain.Entities;

namespace Store.Application.Dtos.Order;

public class OrderStatusMessage
{
    public Guid OrderId { get; set; }
    public OrderStatus Status { get; set; }
}