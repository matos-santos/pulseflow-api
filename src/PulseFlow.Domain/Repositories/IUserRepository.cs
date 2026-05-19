using PulseFlow.Domain.Entities;

namespace PulseFlow.Domain.Repositories;

public interface IUserRepository : IRepository<User, Guid>
{
}
