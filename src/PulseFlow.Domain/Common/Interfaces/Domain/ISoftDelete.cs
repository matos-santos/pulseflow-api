namespace PulseFlow.Domain.Common.Interfaces.Domain;

/// <summary>
/// Interface for entities that support soft delete
/// </summary>
public interface ISoftDelete
{
    /// <summary>
    /// Indicates if the entity is deleted
    /// </summary>
    bool IsDeleted { get; set; }

    /// <summary>
    /// The date and time when the entity was deleted
    /// </summary>
    DateTime? DeletedAt { get; set; }

    /// <summary>
    /// The user who deleted the entity
    /// </summary>
    string? DeletedBy { get; set; }

    /// <summary>
    /// Marks the entity as deleted
    /// </summary>
    void Delete(string? deletedBy = null);

    /// <summary>
    /// Restores a deleted entity
    /// </summary>
    void Restore();

}
