namespace Store.Application.ModelsDto.Products;

public class CreateProductDto
{
    public required string Name { get; init; }
    public decimal Price { get; set; }
    public int Amount { get; set; }
    public required string Composition { get; init; }
    public required string Producer { get; init; }
    public required IEnumerable<Guid> Categories { get; init; }

}