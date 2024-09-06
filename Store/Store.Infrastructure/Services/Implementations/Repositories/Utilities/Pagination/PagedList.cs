using Microsoft.EntityFrameworkCore;

namespace Store.Infrastructure.Services.Implementations.Repositories.Utilities.Pagination;

public class PagedList<T>
{
    private PagedList(List<T> items, int page, int pageSize, int totalCount)
    {
        Items = items;
        Page = page;
        PageSize = pageSize;
        TotalCount = totalCount;
    }
    
    public List<T> Items { get;  }
    private int Page { get; }
    private int PageSize { get; }
    private int TotalCount { get; }

    public bool HasNextPage => Page * PageSize < TotalCount;
    public bool HasPreviousPage => PageSize > 1;

    public static async Task<PagedList<T>> CreateAsync(IQueryable<T> queryable, int page, int pageSize)
    {
        var totalCount = await queryable.CountAsync();
        var items = await queryable.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
        return new PagedList<T>(items, page, pageSize, totalCount);
    }
}