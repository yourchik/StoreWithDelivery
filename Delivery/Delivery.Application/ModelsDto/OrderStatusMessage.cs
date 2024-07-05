namespace Delivery.Application.ModelsDto;

public class OrderStatusMessage
{
    public Guid OrderId { get; set; }
    public OrderStatus Status { get; set; }
}