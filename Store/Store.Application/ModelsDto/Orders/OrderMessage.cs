using Contracts.Enum;
using Store.Domain.Entities;

namespace Store.Application.ModelsDto.Orders;

public class OrderMessage
{
    public Guid Id { get; set; }
    public required string Address { get; set; }
    public OrderStatus Status { get; set; }
}