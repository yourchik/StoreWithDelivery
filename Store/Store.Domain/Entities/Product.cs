namespace Store.Domain.Entities;

public class Product : BaseEntity
{
    public string Name { get; set; }
    public decimal Price { get; set; }
    public int Amount  { get; set; }
    public string Composition { get; set; }
    public string Producer { get; set; }
    public IEnumerable<ProductsCategory> Categories { get; set; }
}