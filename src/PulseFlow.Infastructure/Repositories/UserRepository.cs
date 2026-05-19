using PulseFlow.Domain.Entities;
using PulseFlow.Infrastructure.Persistence;

namespace PulseFlow.Infrastructure.Repositories;

public class UserRepository : BaseRepository<User, Guid>, IUserRepository
{
    public UserRepository(ApplicationDbContext context) : base(context)
    {
    }
}
