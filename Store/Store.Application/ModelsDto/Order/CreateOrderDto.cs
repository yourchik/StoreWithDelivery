using Store.Application.ModelsDto.Product;

namespace Store.Application.ModelsDto.Order;

public class CreateOrderDto
{
    public string Address { get; set; }
    public List<ProductInOrderDto> Products { get; set; }
}