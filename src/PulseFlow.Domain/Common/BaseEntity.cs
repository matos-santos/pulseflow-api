using Medo;

namespace PulseFlow.Domain.Common;

/// <summary>
/// Base class for all entities with a specific key type
/// </summary>
/// <typeparam name="TKey">The type of the entity's primary key</typeparam>
public abstract class BaseEntity<TKey> : IEntity<TKey> where TKey : notnull
{
    public TKey Id { get; private set; }

    protected BaseEntity()
    {
        // Only works if TKey is Guid
        if (typeof(TKey) == typeof(Guid))
        {
            Id = (TKey)(object)Uuid7.NewUuid7().ToGuid();
        }
        else
        {
            Id = default!;
        }
    }

    protected BaseEntity(TKey id)
    {
        Id = id;
    }

    public override bool Equals(object? obj)
    {
        if (obj is not BaseEntity<TKey> other)
            return false;

        if (ReferenceEquals(this, other))
            return true;

        if (GetType() != other.GetType())
            return false;

        if (EqualityComparer<TKey>.Default.Equals(Id, default) || 
            EqualityComparer<TKey>.Default.Equals(other.Id, default))
            return false;

        return EqualityComparer<TKey>.Default.Equals(Id, other.Id);
    }

    public override int GetHashCode()
    {
        return (GetType().ToString() + Id).GetHashCode();
    }

    public static bool operator ==(BaseEntity<TKey>? a, BaseEntity<TKey>? b)
    {
        if (a is null && b is null)
            return true;

        if (a is null || b is null)
            return false;

        return a.Equals(b);
    }

    public static bool operator !=(BaseEntity<TKey>? a, BaseEntity<TKey>? b)
    {
        return !(a == b);
    }
}
