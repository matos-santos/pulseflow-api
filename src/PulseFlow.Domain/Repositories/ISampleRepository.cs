using PulseFlow.Domain.Entities;

namespace PulseFlow.Domain.Repositories;

/// <summary>
/// Repository interface for SampleEntity
/// </summary>
public interface ISampleRepository : IRepository<SampleEntity, Guid>
{
    Task<IReadOnlyList<SampleEntity>> GetActiveAsync(CancellationToken cancellationToken = default);
    Task<SampleEntity?> GetByNameAsync(string name, CancellationToken cancellationToken = default);
}
