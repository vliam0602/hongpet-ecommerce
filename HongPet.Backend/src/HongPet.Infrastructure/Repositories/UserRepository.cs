using HongPet.Domain.Entities;
using HongPet.Domain.Repositories.Abstractions;
using HongPet.Infrastructure.Database;
using HongPet.Infrastructure.Repositories.Commons;
using Microsoft.EntityFrameworkCore;

namespace HongPet.Infrastructure.Repositories;
public class UserRepository : GenericRepository<User>, IUserRepository
{
    public UserRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<bool> IsEmailExistAsync(string email)
    {
        return await _dbSet.AnyAsync(u => u.Email == email);
    }
}
