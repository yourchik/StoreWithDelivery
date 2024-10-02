using Store.Application.ModelsDto.Products;

namespace Store.Application.ModelsDto.Orders;

public class CreateOrderDto
{
    public required string Address { get; init; }
    public required IEnumerable<ProductInOrderDto> Products { get; init; }
}