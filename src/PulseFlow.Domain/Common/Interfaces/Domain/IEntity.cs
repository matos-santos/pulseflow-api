namespace PulseFlow.Domain.Common.Interfaces.Domain;

/// <summary>
/// Base interface for all entities
/// </summary>
public interface IEntity
{
}

/// <summary>
/// Base interface for entities with a specific key type
/// </summary>
/// <typeparam name="TKey">The type of the entity's primary key</typeparam>
public interface IEntity<Guid> : IEntity where Guid : notnull
{
    Guid Id { get; }
}
