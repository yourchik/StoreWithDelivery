using Store.Domain.Entities;

namespace Store.Domain.Repositories.Utilities;

public abstract record BaseFilter<T> where T : BaseEntity
{
    public IQueryable<T> ApplyFilter(IQueryable<T> query) => 
        ApplySpecificFilters(query);
    
    
    protected abstract IQueryable<T> ApplySpecificFilters(IQueryable<T> query);
}