namespace PulseFlow.Application.UseCase.Auth.Login;

public record LoginOutput(
    string AccessToken,
    string RefreshToken,
    DateTime AccessTokenExpiresAt,
    DateTime RefreshTokenExpiresAt,
    UserInfo User
    );

public record UserInfo(
    Guid Id,
    string Name,
    string Email,
    string Role,
    Guid CompanyId,
    string CompanyName,
    bool IsActive
    );