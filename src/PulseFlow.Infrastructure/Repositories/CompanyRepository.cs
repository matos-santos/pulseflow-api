using PulseFlow.Domain.Entities;
using PulseFlow.Domain.Repositories;
using PulseFlow.Infrastructure.Persistence;

namespace PulseFlow.Infrastructure.Repositories;

public class CompanyRepository : BaseRepository<Companies, Guid>, ICompanyRepository
{
    public CompanyRepository(ApplicationDbContext context) : base(context)
    {
    }
}
