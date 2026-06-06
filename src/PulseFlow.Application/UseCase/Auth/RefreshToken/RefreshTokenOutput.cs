namespace PulseFlow.Application.UseCase.Auth.RefreshToken;

public record RefreshTokenOutput(
    string AccessToken,
    string RefreshToken,
    DateTime AccessTokenExpiresAt,
    DateTime RefreshTokenExpiresAt
    );

