using Store.Application.Services.Implementations.Results;
using Store.Application.Services.Interfaces.Results;
using Store.Domain.Entities;

namespace Store.Application.Services.Interfaces.Entities;

public interface IBasketService
{
    Task<EntityResult<Basket>> GetOrCreate();
    Task<EntityResult<Basket>> AddProduct(Guid productId);
    Task<EntityResult<Basket>> DeleteProduct(Guid productId);
    Task<IResult> ClearBasket();
}