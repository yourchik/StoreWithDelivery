using Store.Domain.Entities;

namespace Store.Domain.Repositories.Interfaces;

public interface IProductRepository : IBaseRepository<Product>
{
    Task<(bool IsSuccess, string ErrorMessage)> UpdateAmountAsync(Product product, int reductionAmount);
}