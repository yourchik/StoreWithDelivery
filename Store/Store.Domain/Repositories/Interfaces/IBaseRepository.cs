using Store.Domain.Entities;
using Store.Domain.Repositories.Utilities;

namespace Store.Domain.Repositories.Interfaces;

public interface IBaseRepository<T> where T : BaseEntity 
{
    Task<(IEnumerable<T> Entities, bool IsSuccess, string ErrorMessage)> GetByFilterAsync(BaseFilter<T> filter, int page, int pageSize);
    Task<(T? Entity, bool IsSuccess, string ErrorMessage)> GetByIdAsync(Guid id);
    Task<(bool IsSuccess, string ErrorMessage)> AddAsync(T entity);
    Task<(bool IsSuccess, string ErrorMessage)> DeleteAsync(Guid id);
}  