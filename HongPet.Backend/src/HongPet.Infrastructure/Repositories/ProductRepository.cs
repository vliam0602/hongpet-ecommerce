using Domain.Entities.Commons;
using HongPet.Application.Commons;
using HongPet.Domain.Entities;
using HongPet.Domain.Repositories.Abstractions;
using HongPet.Infrastructure.Database;
using HongPet.Infrastructure.Repositories.Commons;
using Microsoft.EntityFrameworkCore;

namespace HongPet.Infrastructure.Repositories;
public class ProductRepository : GenericRepository<Product>, IProductRepository
{
    public ProductRepository(AppDbContext context) : base(context)
    {
    }

    public override async Task<IEnumerable<Product>> GetAllAsync()
    {
        return await _dbSet
                        .Include(x => x.Variants)
                        .ToListAsync();
    }

    public override async Task<IPagedList<Product>> GetPagedAsync(int pageIndex, int pageSize)
    {
        var totalCount = await _dbSet.CountAsync();
        var items = await _dbSet
                                .Include(x => x.Variants)
                                .Skip((pageIndex - 1) * pageSize)
                                .Take(pageSize)
                                .ToListAsync();
        return new PagedList<Product>(items, totalCount, pageIndex, pageSize);
    }
}
