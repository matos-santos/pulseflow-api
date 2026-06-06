using PulseFlow.Domain.Entities;
using System.Security.Claims;

namespace PulseFlow.Application.Common.Interfaces;

public interface IJwtService
{
    (string accessToken, string jti) GenerateAccessToken(User user);
    string GenerateRefreshToken();
    ClaimsPrincipal? GetPrincipalFromExpiredToken(string token);
}
