using Microsoft.EntityFrameworkCore;
using PulseFlow.Domain.Entities;
using PulseFlow.Domain.Repositories;
using PulseFlow.Infrastructure.Persistence;

namespace PulseFlow.Infrastructure.Repositories;

public class UserRepository : BaseRepository<User, Guid>, IUserRepository
{
    public UserRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<User> GetUserByEmail(string email)
    {
        var user = await DbSet.Include(x => x.Company).FirstOrDefaultAsync(u => u.Email == email);
        
        if (user == null)
        {
            throw new KeyNotFoundException($"E-mail ou senha inválidos para o usuário.");
        }

        return user;
    }
}
