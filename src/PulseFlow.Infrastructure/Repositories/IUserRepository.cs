using PulseFlow.Domain.Entities;
using PulseFlow.Domain.Repositories;

namespace PulseFlow.Infrastructure.Repositories;

public interface IUserRepository : IRepository<User, Guid>
{
}
