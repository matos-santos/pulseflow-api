using PulseFlow.Domain.Common;
using PulseFlow.Domain.Enums;

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

    public Company? Company { get; set; }
    public List<RefreshToken>? RefreshTokens { get; set; } = new List<RefreshToken>();

    private User() : base() { }

    public User(Company company, string email, 
        string firstName, 
        string lastName, 
        string passwordHash, 
        bool isActive, 
        string role,
        string? phoneNumber 
        )
        : base(Guid.NewGuid())
    {
        var roleToDb = RoleExtensions.ToDbString((Role)Enum.Parse(typeof(Role), role));

        Company = company;
        CompanyId = company.Id;
        Email = email;
        FirstName = firstName;
        LastName = lastName;
        PhoneNumber = phoneNumber;
        PasswordHash = passwordHash;
        IsActive = isActive;
        Role = roleToDb;
    }

    public void UpdateProfile(
        string email, 
        string firstName, 
        string lastName, 
        string passwordHash,
        bool isActive,
        string role,
        string? phoneNumber 
        )
    {
        Email = email;
        FirstName = firstName;
        LastName = lastName;
        PhoneNumber = phoneNumber;
        PasswordHash = passwordHash;
        IsActive = isActive;
        Role = RoleExtensions.ToDbString((Role)Enum.Parse(typeof(Role), role));
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
