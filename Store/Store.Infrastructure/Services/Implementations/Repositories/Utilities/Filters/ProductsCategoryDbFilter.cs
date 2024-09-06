using Store.Domain.Entities;
using Store.Domain.Repositories.Utilities;

namespace Store.Infrastructure.Services.Implementations.Repositories.Utilities.Filters;

public record ProductsCategoryDbFilter(
    string? Name)
    : BaseFilter<ProductsCategory>
{
    
    protected override IQueryable<ProductsCategory> ApplySpecificFilters(IQueryable<ProductsCategory> query)
    {
        if (!string.IsNullOrEmpty(Name))
            query = query.Where(o => o.Name.Contains(Name));
        return query;
    }
}