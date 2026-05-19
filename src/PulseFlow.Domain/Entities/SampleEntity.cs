using PulseFlow.Domain.Common;

namespace PulseFlow.Domain.Entities;

/// <summary>
/// Sample entity to demonstrate the domain model structure
/// </summary>
public class SampleEntity : AuditableEntity<Guid>
{
    public string Name { get; private set; } = string.Empty;
    public string? Description { get; private set; }
    public bool IsActive { get; private set; }

    // EF Core requires a parameterless constructor
    private SampleEntity() : base()
    {
    }

    public SampleEntity(string name, string? description = null) : base(Guid.NewGuid())
    {
        Name = name;
        Description = description;
        IsActive = true;
    }

    public void UpdateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name cannot be empty", nameof(name));

        Name = name;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateDescription(string? description)
    {
        Description = description;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Activate()
    {
        IsActive = true;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Deactivate()
    {
        IsActive = false;
        UpdatedAt = DateTime.UtcNow;
    }
}
