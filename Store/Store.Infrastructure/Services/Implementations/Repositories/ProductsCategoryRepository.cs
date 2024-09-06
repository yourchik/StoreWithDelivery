using Store.Domain.Entities;
using Store.Infrastructure.Services.Implementations.Repositories.EFCoreRepository;

namespace Store.Infrastructure.Services.Implementations.Repositories;

public class ProductsCategoryRepository(ApplicationDbContext dbContext) 
    : BaseRepository<ProductsCategory>(dbContext) { }