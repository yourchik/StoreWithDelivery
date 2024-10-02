namespace Store.Domain.Entities;

public class Product : BaseEntity
{
    public string? Name { get; set; }
    public required decimal Price { get; set; }
    public required int Amount  { get; set; }
    public required string Composition { get; set; }
    public required string Producer { get; set; }
    public required IEnumerable<ProductsCategory> Categories { get; set; }
}