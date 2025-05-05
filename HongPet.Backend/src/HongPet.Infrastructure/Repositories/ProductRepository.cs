using HongPet.Domain.Entities;
using HongPet.Domain.Repositories.Abstractions;
using HongPet.Infrastructure.Database;
using HongPet.Infrastructure.Repositories.Commons;
using HongPet.SharedViewModels.Generals;
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
                        .OrderByDescending(x => x.CreatedDate)
                        .ToListAsync();
    }

    public async Task<IPagedList<Product>> GetPagedProductsAsync
        (int pageIndex = 1, int pageSize = 10, string? keyword = "",
        List<string>? category = null)
    {
        
        var query = _dbSet
            .Include(x => x.Variants)
            .Include(x => x.Categories)
            .Where(x => x.DeletedDate == null)
            .AsQueryable();

        if (category != null && category.Any())
        {
            query = query.Where(x => x.Categories
                    .Any(c => category.Contains(c.Name)));
        }

        if (!string.IsNullOrEmpty(keyword))
        {
            query = query.Where(x => x.Name.Contains(keyword)
                        || x.Categories.Any(c => c.Name.Contains(keyword)));
        }
        
        var totalCount = await query.CountAsync();

        var items = await query
            .OrderByDescending(x => x.CreatedDate)
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new PagedList<Product>(items, totalCount, pageIndex, pageSize);
    }

    public async Task<Product?> GetProductDetailAsync(Guid id)
    {
        return await _dbSet
            .Include(x => x.Reviews)
            .Include(x => x.Categories)
            .Include(x => x.Variants)
                .ThenInclude(x => x.AttributeValues)
                    .ThenInclude(x => x.Attribute)
                        .Include(x => x.Images)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<IPagedList<Product>> GetProductsByCategoryAsync(string categoryName, int pageIndex = 1, int pageSize = 10, string? keyword = "")
    {
        var products = _dbSet
            .Where(x => x.Categories.Any(c => c.Name == categoryName))
            .OrderByDescending(x => x.CreatedDate);

        return await base.ToPaginationAsync(products, pageIndex, pageSize, keyword);
    }

    public async Task<ProductAttributeValue?> GetAttributeValuePairAsync
        (string attributeName, string attributeValue)
    {
        return await _context.ProductAttributeValues
            .Include(x => x.Attribute)
            .FirstOrDefaultAsync(x => 
                x.Attribute.Name == attributeName 
                && x.Value == attributeValue);
    }

    public async Task<ProductAttribute?> GetAttributeByNameAsync(string attributeName)
    {
        return await _context.ProductAttributes
            .FirstOrDefaultAsync(x => x.Name == attributeName);
    }
}
