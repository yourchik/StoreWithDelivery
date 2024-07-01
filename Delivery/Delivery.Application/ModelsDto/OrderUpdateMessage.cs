namespace Delivery.Application.ModelsDto;

public class OrderUpdateMessage
{
    public Guid OrderId { get; set; }
    public OrderStatus Status { get; set; }
}