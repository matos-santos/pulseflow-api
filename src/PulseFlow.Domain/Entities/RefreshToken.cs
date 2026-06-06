namespace PulseFlow.Domain.Entities;

public class RefreshToken : AuditableEntity<Guid>
{
    public Guid UserId { get; set; }
    public string Token { get; private set; } = string.Empty;
    public string? AccessTokenJti { get; private set; }
    public DateTime ExpiresAt { get; private set; }
    public bool IsRevoked { get; private set; }
    public string? RevokedReason { get; private set; }
    public DateTime? RevokedAt { get; private set; }
    public string? ReplacedByToken { get; set; }
    public User? User { get; set; }
    
    private RefreshToken() : base() { }
    
    public RefreshToken(Guid userId, string token, DateTime expiresAt, string? accessTokenJti = null) : base(Guid.NewGuid())
    {
        UserId = userId;
        Token = token;
        ExpiresAt = expiresAt;
        AccessTokenJti = accessTokenJti;
    }

    public bool IsExpired => DateTime.UtcNow >= ExpiresAt;
    public bool IsActive => !IsRevoked && !IsExpired;


    public void UpdateToken(string token, DateTime expiresAt)
    {
        Token = token;
        ExpiresAt = expiresAt;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Revoke(string reason, string? replacedByToken = null)
    {
        IsRevoked = true;
        RevokedReason = reason;
        RevokedAt = DateTime.UtcNow;
        ReplacedByToken = replacedByToken;
        
    }
}
