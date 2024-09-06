using Store.Domain.Entities;
using Store.Domain.Repositories.Utilities;

namespace Store.Infrastructure.Services.Implementations.Repositories.Utilities.Filters;

public record ProductDbFilers(
    string? Name,
    decimal? Price,
    int? Amount,
    string? Composition,
    string? Producer,
    IEnumerable<ProductsCategory>? Categories
) : BaseFilter<Product>
{
    protected override IQueryable<Product> ApplySpecificFilters(IQueryable<Product> query)
    {
        if (!string.IsNullOrEmpty(Name))
            query = query.Where(o => o.Name.Contains(Name));
        
        if (Price.HasValue)
            query = query.Where(o => o.Price == Price.Value);
        
        if (Amount.HasValue)
            query = query.Where(o => o.Amount == Amount.Value);
        
        if (!string.IsNullOrEmpty(Composition))
            query = query.Where(o => o.Composition.Contains(Composition));
        
        if (!string.IsNullOrEmpty(Producer))
            query = query.Where(o => o.Producer.Contains(Producer));
        
        if (Categories != null && Categories.Any())
            query = query.Where(o => o.Categories.Any(c => Categories.Contains(c)));

        return query;
    }
}