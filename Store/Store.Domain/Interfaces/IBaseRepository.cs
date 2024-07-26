namespace Store.Domain.Interfaces;

public interface IBaseRepository<T> where T : class
{
    Task<(IEnumerable<T> Entities, bool IsSuccess, string ErrorMessage)> GetAllAsync();
    Task<(T? Entity, bool IsSuccess, string ErrorMessage)> GetByIdAsync(Guid id);
    Task<(bool IsSuccess, string ErrorMessage)> AddAsync(T entity);
    Task<(bool IsSuccess, string ErrorMessage)> DeleteAsync(Guid id);
}