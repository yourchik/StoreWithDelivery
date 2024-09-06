namespace Store.Application.ModelsDto.Product;

public class CreateProductDto
{
    public string Name { get; set; }
    public decimal Price { get; set; }
    public int Amount { get; set; }
    public string Сomposition { get; set; }
    public string Producer { get; set; }
    public List<Guid> Categories { get; set; }

}