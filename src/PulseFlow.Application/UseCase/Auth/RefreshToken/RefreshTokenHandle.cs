using MediatR;
using Microsoft.Extensions.Options;
using PulseFlow.Application.Common.Interfaces;
using PulseFlow.Application.Common.Models;
using PulseFlow.Domain.Repositories;
using System.Security.Claims;

namespace PulseFlow.Application.UseCase.Auth.RefreshToken;

public class RefreshTokenHandle(
    IUserRepository userRepository,
    IRefreshTokenRepository refreshTokenRepository,
    IJwtService jwtService,
    IOptions<JwtSettings> jwtSettings
    )
    : IRequestHandler<RefreshTokenInput, RefreshTokenOutput> 
{
    public async  Task<RefreshTokenOutput> Handle(RefreshTokenInput request, CancellationToken cancellationToken)
    {
        var principal = jwtService.GetPrincipalFromExpiredToken(request.AccessToken);
        if (principal == null)
        {
            throw new UnauthorizedAccessException("Token inválido.");
        }

        var userIdClaim = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdClaim))
        {
            throw new UnauthorizedAccessException("Token inválido.");
        }

        var userId = Guid.Parse(userIdClaim);
        var user = await userRepository.GetByIdAsync(userId, cancellationToken);
        if (user == null)
        {
            throw new UnauthorizedAccessException("Token inválido.");
        }

        var refreshToken = await refreshTokenRepository.GetByTokenAsync(request.RefreshToken, cancellationToken);
        if (refreshToken == null || refreshToken.UserId != userId)
        {
            throw new UnauthorizedAccessException("Token inválido.");
        }

        throw new NotImplementedException();
    }
}
