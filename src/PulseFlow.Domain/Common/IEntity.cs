namespace PulseFlow.Domain.Common;

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
public interface IEntity<TKey> : IEntity where TKey : notnull
{
    TKey Id { get; }
}
