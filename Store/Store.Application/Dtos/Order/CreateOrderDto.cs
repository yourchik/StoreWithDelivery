using Store.Application.Dtos.Product;
using Store.Domain.Entities;

namespace Store.Application.Dtos.Order;

public class CreateOrderDto
{
    public string Address { get; set; }
    public List<Guid> ProductsGuid { get; set; }
    public OrderStatus Status { get; set; }
}