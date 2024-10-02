using Contracts.Enum;
using Store.Domain.Entities;
using Store.Domain.Repositories.Utilities;

namespace Store.Infrastructure.Services.Implementations.Repositories.Utilities.Filters;

public record OrderDbFilter(
    string? Address,
    OrderStatus? Status
) : BaseFilter<Order>
{
    protected override IQueryable<Order> ApplySpecificFilters(IQueryable<Order> query)
    {
        if (!string.IsNullOrEmpty(Address))
            query = query.Where(o => o.Address.Contains(Address));
        
        if (Status.HasValue)
            query = query.Where(o => o.Status == Status.Value);
        
        return query;
    }
}