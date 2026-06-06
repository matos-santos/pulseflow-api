using Microsoft.EntityFrameworkCore;
using PulseFlow.Domain.Entities;
using PulseFlow.Domain.Repositories;
using PulseFlow.Infrastructure.Persistence;

namespace PulseFlow.Infrastructure.Repositories;

public class RefreshTokenRepository : BaseRepository<RefreshToken, Guid>, IRefreshTokenRepository
{
    public RefreshTokenRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<RefreshToken?> GetByTokenAsync(string token, CancellationToken cancellationToken)
    {
        return await DbSet.FirstOrDefaultAsync(rt => rt.Token == token, cancellationToken);
    }
}
