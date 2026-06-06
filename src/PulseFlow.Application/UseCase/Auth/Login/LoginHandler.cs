using MediatR;
using Microsoft.Extensions.Options;
using PulseFlow.Application.Common.Interfaces;
using PulseFlow.Application.Common.Models;
using PulseFlow.Domain.Common.Helpers;
using PulseFlow.Domain.Repositories;

namespace PulseFlow.Application.UseCase.Auth.Login;

public class LoginHandler(
    IUserRepository userRepository,
    IJwtService jwtService,
    IOptions<JwtSettings> jwtSettings,
    IRefreshTokenRepository refreshTokenRepository,
    IUnitOfWork unitOfWork
    ) : IRequestHandler<LoginInput, LoginOutput>
{
    public async Task<LoginOutput> Handle(LoginInput request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetUserByEmail(request.Email);
        if (user is null || !PasswordHelper.VerifyPassword(request.Password, user.PasswordHash))
        {
            throw new UnauthorizedAccessException("Email ou senha inválidos.");
        }

        if (user.IsActive == false)
        {
            throw new UnauthorizedAccessException("Usuário inativo. Entre em contato com o administrador.");
        }

        var (accessToken, jti) = jwtService.GenerateAccessToken(user);
        var refreshToken = jwtService.GenerateRefreshToken();

        var refreshTokenEntity = new Domain.Entities.RefreshToken(user.Id, refreshToken, DateTime.UtcNow.AddDays(jwtSettings.Value.RefreshTokenExpirationDays));

        await refreshTokenRepository.AddAsync(refreshTokenEntity);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return new LoginOutput(accessToken, refreshToken, DateTime.UtcNow.AddMinutes(jwtSettings.Value.AccessTokenExpirationMinutes),
            refreshTokenEntity.ExpiresAt,
            new UserInfo(
                user.Id,
                user.GetFullName(),
                user.Email,
                user.Role,
                user.CompanyId,
                user.Company?.Name ?? string.Empty,
                user.IsActive
            )
        );
    }

}