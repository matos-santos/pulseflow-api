namespace PulseFlow.Domain.Enums;

public enum Role
{
    SUPER_ADMIN,
    ADMIN,
    USER,
    SUPPORT
}

public static class RoleExtensions
{
    public static string ToDbString(this Role role)
    {
        return role switch
        {
            Role.SUPER_ADMIN => "SUPER_ADMIN",
            Role.ADMIN => "ADMIN",
            Role.SUPPORT => "SUPPORT",
            Role.USER => "USER",
            _ => throw new ArgumentOutOfRangeException(nameof(role), role, null)
        };
    }

    public static Role FromDbString(string value)
    {
        return value switch
        {
            "SUPER_ADMIN" => Role.SUPER_ADMIN,
            "ADMIN" => Role.ADMIN,
            "SUPPORT" => Role.SUPPORT,
            "USER" => Role.USER,
            _ => throw new ArgumentException($"Invalid role value: {value}", nameof(value))
        };
    }
}