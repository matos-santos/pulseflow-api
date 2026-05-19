namespace PulseFlow.Domain.Common;

/// <summary>
/// Base class for soft-deletable entities
/// </summary>
/// <typeparam name="TKey">The type of the entity's primary key</typeparam>
public abstract class SoftDeletableEntity<TKey> : AuditableEntity<TKey>, ISoftDeletable 
    where TKey : notnull
{
    public bool IsDeleted { get; set; }
    public DateTime? DeletedAt { get; set; }
    public string? DeletedBy { get; set; }

    protected SoftDeletableEntity() : base()
    {
    }

    protected SoftDeletableEntity(TKey id) : base(id)
    {
    }

    public void Delete(string? deletedBy = null)
    {
        IsDeleted = true;
        DeletedAt = DateTime.UtcNow;
        DeletedBy = deletedBy;
    }

    public void Restore()
    {
        IsDeleted = false;
        DeletedAt = null;
        DeletedBy = null;
    }
}
