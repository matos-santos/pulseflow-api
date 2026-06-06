using Microsoft.EntityFrameworkCore;
using PulseFlow.Domain.Entities;
using PulseFlow.Domain.Repositories;
using PulseFlow.Infrastructure.Persistence;

namespace PulseFlow.Infrastructure.Repositories;

/// <summary>
/// Repository implementation for SampleEntity with specific queries
/// </summary>
public class SampleRepository : BaseRepository<SampleEntity, Guid>, ISampleRepository
{
    public SampleRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<IReadOnlyList<SampleEntity>> GetActiveAsync(CancellationToken cancellationToken = default)
    {
        return await DbSet
            .Where(s => s.IsActive)
            .OrderBy(s => s.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<SampleEntity?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        return await DbSet
            .FirstOrDefaultAsync(s => s.Name == name, cancellationToken);
    }
}
