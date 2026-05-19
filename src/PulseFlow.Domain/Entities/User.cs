using PulseFlow.Domain.Common;

namespace PulseFlow.Domain.Entities;

public class User : AuditableEntity<Guid>
{
    public Guid CompanyId { get; set; }
    public string Email { get; private set; } = string.Empty;
    public string FirstName { get; private set; } = string.Empty;
    public string LastName { get; private set; } = string.Empty;
    public string? PhoneNumber { get; private set; }
    public string PasswordHash { get; private set; } = string.Empty;
    public bool IsActive { get; private set; }

    public string Role { get; private set; } = string.Empty;

    public Companies Company { get; set; }
    public List<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();

    private User() : base() { }

    public User(string email, string firstName, string lastName, string? phoneNumber, string passwordHash, bool isActive, string role)
        : base(Guid.NewGuid())
    {
        Email = email;
        FirstName = firstName;
        LastName = lastName;
        PhoneNumber = phoneNumber;
        PasswordHash = passwordHash;
        IsActive = isActive;
        Role = role;
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
