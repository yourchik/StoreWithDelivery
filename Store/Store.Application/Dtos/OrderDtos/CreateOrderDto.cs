using Store.Application.Dtos.Product;
using Store.Domain.Entities;

namespace Store.Application.Dtos.OrderDtos;

public class CreateOrderDto
{
    public string Address { get; set; }
    public List<ProductInOrderDto> Products { get; set; }
    public OrderStatus Status { get; set; }
}