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

    public override async Task<IPagedList<Product>> GetPagedAsync
        (int pageIndex = 1, int pageSize = 10, string? keyword = "")
    {
        var totalCount = await _dbSet.CountAsync();

        var items = await _dbSet
            .Where(x => string.IsNullOrEmpty(keyword)
                        || x.Name.Contains(keyword)
                        || x.Categories.Any(c => c.Name.Contains(keyword)))
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new PagedList<Product>(items, totalCount, pageIndex, pageSize);
    }

    public async Task<Product?> GetProductDetailAsync(Guid id)
    {
        return await _dbSet
            .Include(x => x.Variants)
            .ThenInclude(x => x.AttributeValues)
            .ThenInclude(x => x.Attribute)
            .Include(x => x.Images)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<IPagedList<Product>> GetProductsByCategoryAsync(string categoryName, int pageIndex = 1, int pageSize = 10, string? keyword = "")
    {
        var products = _dbSet
            .Where(x => x.Categories.Any(c => c.Name == categoryName));

        return await base.ToPaginationAsync(products, pageIndex, pageSize, keyword);
    }
}
