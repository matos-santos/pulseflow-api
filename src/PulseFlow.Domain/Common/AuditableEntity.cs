namespace PulseFlow.Domain.Common;

/// <summary>
/// Base class for auditable entities
/// </summary>
/// <typeparam name="TKey">The type of the entity's primary key</typeparam>
public abstract class AuditableEntity<Guid> : BaseEntity<Guid>, IAuditableEntity 
    where Guid : notnull
{
    public DateTime CreatedAt { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public string? UpdatedBy { get; set; }

    protected AuditableEntity() : base()
    {
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    protected AuditableEntity(Guid id) : base(id)
    {
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }
}
