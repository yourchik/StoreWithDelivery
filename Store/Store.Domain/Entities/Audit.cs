namespace Store.Domain.Entities;

public class Audit : BaseEntity
{
    public required string EntityName { get; set; }
    
    public required Guid EntityId { get; set; }
    
    public string? PropertyName { get; set; }
    
    public string? OldValue { get; set; }
    
    public string? NewValue { get; set; }
    
    public DateTime ChangedAt { get; set; }
    
    public Guid UserId { get; set; }
}