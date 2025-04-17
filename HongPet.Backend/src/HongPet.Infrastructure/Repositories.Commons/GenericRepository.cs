
using Domain.Entities.Commons;
using HongPet.Application.Commons;
using HongPet.Domain.Repositories.Abstraction.Commons;
using HongPet.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace HongPet.Infrastructure.Repositories.Commons;

public class GenericRepository<TEntity> : IGenericRepository<TEntity> 
    where TEntity : BaseEntity
{
    private readonly AppDbContext _context;
    private readonly DbSet<TEntity> _dbSet;

    public GenericRepository(AppDbContext context)
    {
        _context = context;
        _dbSet = _context.Set<TEntity>();
    }

    public async Task<TEntity?> GetByIdAsync(Guid id)
    {
        return await _dbSet.FindAsync(id);
    }

    public async Task<IEnumerable<TEntity>> GetAllAsync()
    {
        return await _dbSet.ToListAsync();
    }

    public async Task AddAsync(TEntity entity)
    {
        await _dbSet.AddAsync(entity);
    }

    public void Update(TEntity entity)
    {
        // to update modified fields only
        _dbSet.Attach(entity);
        _context.Entry(entity).State = EntityState.Modified;        
    }

    public async Task DeleteAsync(Guid id)
    {
        var entity = await GetByIdAsync(id);
        if (entity == null)
        {
            throw new KeyNotFoundException($"Entity with id {id} not found.");
        }
        _dbSet.Remove(entity);
    }

    public async Task<IEnumerable<TEntity>> GetAsync
        (Expression<Func<TEntity, bool>> query)
    {
        return await _dbSet.Where(query).ToListAsync();
    }

    public async Task<IPagedList<TEntity>> GetPagedAsync
        (int pageIndex, int pageSize)
    {
        var totalCount = await _dbSet.CountAsync();
        var items = await _dbSet.Skip((pageIndex - 1) * pageSize)
                                .Take(pageSize)
                                .ToListAsync();
        return new PagedList<TEntity>(items, totalCount, pageIndex, pageSize);
    }
}
