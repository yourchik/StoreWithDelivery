using Store.Application.Services.Factories;
using Store.Application.Services.Interfaces.AuditManagement;
using Store.Application.Services.Interfaces.Results;
using Store.Domain.Entities;
using Store.Domain.Repositories.Interfaces;

namespace Store.Application.Services.Implementations.AuditManagement;

public class AuditService(IAuditRepository auditRepository) : IAuditService
{
    public async Task<IResult> AuditChange(string entityName, Guid entityId, Guid userId, 
        string? propertyName = null, string? oldValue = null, string? newValue = null)
    {
        var audit = new Audit
        {
            EntityName = entityName,
            EntityId = entityId,
            PropertyName = propertyName,
            OldValue = oldValue,
            NewValue = newValue,
            ChangedAt = DateTime.UtcNow,
            UserId = userId
        };

        var (isSuccess, errorMessage) = await auditRepository.CreateAsync(audit);
        if (!isSuccess)
            return ResultFactory.CreateResult(isSuccess, errorMessage);

        return ResultFactory.CreateResult(isSuccess, string.Empty);
    }
}