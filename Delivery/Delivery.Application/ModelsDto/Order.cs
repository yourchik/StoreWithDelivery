namespace Delivery.Application.ModelsDto;

public class Order
{
        public Guid Id { get; set; }
        public string Address { get; set; }
        public OrderStatus Status { get; set; }
}

