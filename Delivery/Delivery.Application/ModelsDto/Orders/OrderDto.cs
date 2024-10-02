using Contracts.Enum;
using Delivery.Domain.Entities;

namespace Delivery.Application.ModelsDto.Orders;

public class OrderDto
{
        public Guid Id { get; set; }
        public required string Address { get; set; }
        public OrderStatus Status { get; set; }
}

