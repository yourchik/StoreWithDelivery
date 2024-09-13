using Store.Domain.Entities;
using Store.Domain.Repositories.Interfaces;
using Store.Infrastructure.Services.Implementations.Repositories.EFCoreRepository;

namespace Store.Infrastructure.Services.Implementations.Repositories;

public class AuditRepository(ApplicationDbContext dbContext) : BaseRepository<Audit>(dbContext), IAuditRepository;