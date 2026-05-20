using PulseFlow.Domain.Common;

namespace PulseFlow.Domain.Entities;

public class Company : AuditableEntity<Guid>
{
    public string Name { get; private set; } = string.Empty;

    public string Slug { get; private set; } = string.Empty;

    public bool IsActive { get; private set; }

    public List<User> Users { get; set; } = new List<User>();

    private Company() : base() { }

    public Company(string name, string slug, bool isActive)
    {
        Name = name;
        Slug = slug;
        IsActive = isActive;
    }

    public void UpdateDetails(string name, string slug)
    {
        Name = name;
        Slug = slug;
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

    public void AddUserDefault(
        string email, 
        string firstName, 
        string lastName, 
        string password, 
        string role,
        string? phoneNumber
        )
    {

        var user = new User(
            this,
            email,
            firstName, 
            lastName, 
            password, 
            true,
            role,
            phoneNumber 
            );
        Users.Add(user);
    }

}
