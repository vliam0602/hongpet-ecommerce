using HongPet.Domain.Entities;
using HongPet.Domain.Repositories.Abstractions;
using HongPet.Infrastructure.Database;
using HongPet.Infrastructure.Repositories.Commons;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace HongPet.Infrastructure.Repositories;
public class UserTokenRepository : GenericRepository<UserToken>, IUserTokenRepository
{
    public UserTokenRepository(AppDbContext context) : base(context)
    {
    }

    public override async Task<IEnumerable<UserToken>> GetAsync(Expression<Func<UserToken, bool>> query)
    {
        return await _dbSet.Include(x => x.User)
                           .Where(query)
                           .ToListAsync();
    }
}
