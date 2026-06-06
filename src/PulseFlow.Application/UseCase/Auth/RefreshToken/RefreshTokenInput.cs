using MediatR;

namespace PulseFlow.Application.UseCase.Auth.RefreshToken;

public record RefreshTokenInput(
    string AccessToken,
    string RefreshToken
    ) : IRequest<RefreshTokenOutput>;

