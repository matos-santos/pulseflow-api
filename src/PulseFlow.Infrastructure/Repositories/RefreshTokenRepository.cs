using PulseFlow.Domain.Entities;
using PulseFlow.Domain.Repositories;
using PulseFlow.Infrastructure.Persistence;

namespace PulseFlow.Infrastructure.Repositories;

public class RefreshTokenRepository : BaseRepository<RefreshToken, Guid>, IRefreshTokenRepository
{
    public RefreshTokenRepository(ApplicationDbContext context) : base(context)
    {
    }
}
