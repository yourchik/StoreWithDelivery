using Microsoft.EntityFrameworkCore;
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
        var paged = await PagedList<T>.CreateAsync(entitiesByFilter, page, pageSize);
        
        if (paged.Items.Count == 0)
            return (paged.Items, false, $"No {nameof(T)} found"); 
        
        return (paged.Items, true, string.Empty);
    }

    public virtual async Task<(T Entity, bool IsSuccess, string ErrorMessage)> GetByIdAsync(Guid id)
    {
        var entity = await DbContext.Set<T>().FindAsync(id);
        if (entity == null)
            return (default!, false, $"{nameof(T)} with ID {id} not found.");

        return (entity, true, string.Empty);
    }

    public virtual async Task<(bool IsSuccess, string ErrorMessage)> CreateAsync(T entity)
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

    public async Task<(IEnumerable<T> Entities, bool IsSuccess, string ErrorMessage)> GetAllAsync()
    {
        var entities = await DbContext.Set<T>().ToListAsync();
        
        if (entities.Count == 0)
            return (entities, false, $"No {nameof(T)} found"); 
        
        return (entities, true, string.Empty);
    }
}