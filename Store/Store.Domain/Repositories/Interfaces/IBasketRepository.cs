using Store.Domain.Entities;

namespace Store.Domain.Repositories.Interfaces;

public interface IBasketRepository : IBaseRepository<Basket>
{
    Task<(bool IsSuccess, string ErrorMessage)> UpdateAsync(Basket basket);
    Task<(Basket Entity, bool IsSuccess, string ErrorMessage)> GetByUserIdAsync(Guid userId);
}