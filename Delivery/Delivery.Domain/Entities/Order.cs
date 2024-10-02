using Contracts.Enum;

namespace Delivery.Domain.Entities;

public class Order : BaseEntity
{
    public required string Address { get; set; }
    public required OrderStatus Status { get; set; }
}