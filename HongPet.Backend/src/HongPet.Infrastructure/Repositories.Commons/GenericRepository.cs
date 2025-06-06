﻿using HongPet.Domain.Entities.Commons;
using HongPet.Domain.Repositories.Abstractions;
using HongPet.Infrastructure.Database;
using HongPet.SharedViewModels.Generals;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace HongPet.Infrastructure.Repositories.Commons;

public class GenericRepository<TEntity> : IGenericRepository<TEntity>
    where TEntity : BaseEntity
{
    protected readonly AppDbContext _context;
    protected readonly DbSet<TEntity> _dbSet;

    public GenericRepository(AppDbContext context)
    {
        _context = context;
        _dbSet = _context.Set<TEntity>();
    }

    public virtual async Task<TEntity?> GetByIdAsync(Guid id)
    {
        return await _dbSet.FindAsync(id);
    }

    public virtual async Task<IEnumerable<TEntity>> GetAllAsync()
    {
        return await _dbSet.OrderByDescending(x => x.CreatedDate).ToListAsync();
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

    public virtual async Task<IEnumerable<TEntity>> GetAsync
        (Expression<Func<TEntity, bool>> query)
    {
        return await _dbSet.Where(query).ToListAsync();
    }

    public virtual async Task<IPagedList<TEntity>> GetPagedAsync
        (int pageIndex = 1, int pageSize = 10, string? keyword = "")
    {
        if (pageIndex < 1) pageIndex = 1;
        if (pageSize < 1) pageSize = 1;

        var query = _dbSet
            .OrderByDescending(x => x.CreatedDate)
            .AsQueryable();

        var totalCount = await query.CountAsync();

        var totalPage = (int)Math.Ceiling((double)totalCount / pageSize);

        if (pageIndex > totalPage) pageIndex = totalPage;

        var items = await query
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new PagedList<TEntity>(items, totalCount, pageIndex, pageSize);
    }

    public async Task<IPagedList<TEntity>> ToPaginationAsync(IQueryable<TEntity> entities, int pageIndex = 1, int pageSize = 10, string? keyword = "")
    {
        if (pageIndex < 1) pageIndex = 1;
        if (pageSize < 1) pageSize = 1;

        var totalCount = entities.Count();

        var totalPage = (int)Math.Ceiling((double)totalCount / pageSize);

        if (pageIndex > totalPage) pageIndex = totalPage;
        

        var items = await entities.Skip((pageIndex - 1) * pageSize)
                            .Take(pageSize)
                            .ToListAsync();

        return new PagedList<TEntity>(items, totalCount, pageIndex, pageSize);
    }

    public async Task<bool> IsExistAsync(Guid id)
    {
        return await _dbSet.AnyAsync(x => x.Id == id);
    }    
}
