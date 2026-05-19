using PulseFlow.Domain.Entities;
using PulseFlow.Domain.Repositories;

namespace PulseFlow.Infrastructure.Repositories;

/// <summary>
/// Repository interface for SampleEntity
/// </summary>
public interface ISampleRepository : IRepository<SampleEntity, Guid>
{
    Task<IReadOnlyList<SampleEntity>> GetActiveAsync(CancellationToken cancellationToken = default);
    Task<SampleEntity?> GetByNameAsync(string name, CancellationToken cancellationToken = default);
}
