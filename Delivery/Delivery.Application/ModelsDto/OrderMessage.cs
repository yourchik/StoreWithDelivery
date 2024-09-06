namespace Delivery.Application.ModelsDto;

public class OrderMessage
{
        public Guid Id { get; set; }
        public string Address { get; set; }
        public IEnumerable<ProductDto> Products { get; set; }
        public OrderStatus Status { get; set; }
}

