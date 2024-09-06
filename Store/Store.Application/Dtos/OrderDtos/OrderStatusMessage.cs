using Store.Domain.Entities;

namespace Store.Application.Dtos.OrderDtos;

public class OrderStatusMessage
{
    public Guid OrderId { get; set; }
    public OrderStatus Status { get; set; }
}