using PulseFlow.Domain.Entities;

namespace PulseFlow.Domain.Repositories;

public interface IRefreshTokenRepository : IRepository<RefreshToken, Guid>
{
    Task<RefreshToken?> GetByTokenAsync(string token, CancellationToken cancellationToken);
}
