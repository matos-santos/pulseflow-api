using PulseFlow.Domain.Common;

namespace PulseFlow.Domain.Entities;

public class RefreshToken : AuditableEntity<Guid>
{
    public Guid UserId { get; set; }
    public string Token { get; private set; } = string.Empty;
    public DateTime ExpiresAt { get; private set; }
    public User? User { get; set; }
    
    private RefreshToken() : base() { }
    
    public RefreshToken(Guid userId, string token, DateTime expiresAt) : base(Guid.NewGuid())
    {
        UserId = userId;
        Token = token;
        ExpiresAt = expiresAt;
    }

    public void UpdateToken(string token, DateTime expiresAt)
    {
        Token = token;
        ExpiresAt = expiresAt;
        UpdatedAt = DateTime.UtcNow;
    }
}
