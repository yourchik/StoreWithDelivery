using Store.Application.Services.Interfaces.Results;

namespace Store.Application.Services.Interfaces.AuditManagement;

public interface IAuditService
{
    Task<IResult> AuditChange(string entityName, Guid entityId, Guid userId, string? propertyName = null, 
        string? oldValue = null, string? newValue = null);
}