using PulseFlow.Domain.Common;

namespace PulseFlow.Domain.Entities;

public class User : AuditableEntity<Guid>
{
    public string Email { get; private set; } = string.Empty;
    public string FirstName { get; private set; } = string.Empty;
    public string LastName { get; private set; } = string.Empty;
    public string? PhoneNumber { get; private set; }
    public string PasswordHash { get; private set; }
    public bool IsActive { get; private set; }

    private User() : base() { }

    public User(string email, string firstName, string lastName, string? phoneNumber, string passwordHash, bool isActive)
        : base(Guid.NewGuid())
    {
        Email = email;
        FirstName = firstName;
        LastName = lastName;
        PhoneNumber = phoneNumber;
        PasswordHash = passwordHash;
        IsActive = isActive;
    }

    public void UpdateProfile(string firstName, string lastName, string? phoneNumber, string email)
    {
        FirstName = firstName;
        LastName = lastName;
        PhoneNumber = phoneNumber;
        Email = email;
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
