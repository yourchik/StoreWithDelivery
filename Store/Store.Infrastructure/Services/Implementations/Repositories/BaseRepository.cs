using Store.Domain.Entities;
using Store.Domain.Repositories.Interfaces;
using Store.Domain.Repositories.Utilities;
using Store.Infrastructure.Services.Implementations.Repositories.EFCoreRepository;
using Store.Infrastructure.Services.Implementations.Repositories.Utilities.Pagination;

namespace Store.Infrastructure.Services.Implementations.Repositories;

public abstract class BaseRepository<T>(ApplicationDbContext dbContext) 
    : IBaseRepository<T> where T : BaseEntity
{
    protected readonly ApplicationDbContext DbContext = dbContext;

    public virtual async Task<(IEnumerable<T> Entities, bool IsSuccess, string ErrorMessage)> GetByFilterAsync(
        BaseFilter<T> filter, int page, int pageSize)
    {
        var entitiesQueryable = DbContext.Set<T>().AsQueryable();
        var entitiesByFilter = filter.ApplyFilter(entitiesQueryable);
        var pagedOrders = await PagedList<T>.CreateAsync(entitiesByFilter, page, pageSize);
        
        if (pagedOrders.Items.Count == 0)
            return (pagedOrders.Items, false, $"No {nameof(T)} found"); 
        
        return (pagedOrders.Items, true, string.Empty);
    }

    public virtual async Task<(T? Entity, bool IsSuccess, string ErrorMessage)> GetByIdAsync(Guid id)
    {
        var entity = await DbContext.Set<T>().FindAsync(id);
        if (entity == null)
            return (null, false, $"{nameof(T)} with ID {id} not found.");

        return (entity, true, string.Empty);
    }

    public virtual async Task<(bool IsSuccess, string ErrorMessage)> AddAsync(T entity)
    { 
        await DbContext.Set<T>().AddAsync(entity);
        await DbContext.SaveChangesAsync();
        return (true, string.Empty);
    }

    public virtual async Task<(bool IsSuccess, string ErrorMessage)> DeleteAsync(Guid id)
    {
        var entity = await DbContext.Set<T>().FindAsync(id);
        if (entity == null)
            return (false, $"{nameof(T)} with ID {id} not found.");

        DbContext.Set<T>().Remove(entity);
        await DbContext.SaveChangesAsync();
        return (true, string.Empty);
    }
}