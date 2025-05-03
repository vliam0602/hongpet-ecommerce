using HongPet.Domain.Entities;
using HongPet.Domain.Repositories.Abstractions;
using HongPet.Infrastructure.Database;
using HongPet.Infrastructure.Repositories.Commons;
using HongPet.SharedViewModels.Generals;
using Microsoft.EntityFrameworkCore;

namespace HongPet.Infrastructure.Repositories;
public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
{
    public CategoryRepository(AppDbContext context) : base(context)
    {
    }

    public override async Task<IPagedList<Category>> GetPagedAsync(
        int pageIndex = 1, int pageSize = 10, string? keyword = "")
    {
        var query = _dbSet
            .Include(x => x.ParentCategory)
            .AsQueryable();

        if (!string.IsNullOrEmpty(keyword))
        {
            query = query.Where(x => x.Name.Contains(keyword));
        }
        var totalCount = await query.CountAsync();

        var items = await query
            .OrderByDescending(x => x.CreatedDate)
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new PagedList<Category>(items, totalCount, pageIndex, pageSize);
    }

    public override async Task<Category?> GetByIdAsync(Guid id)
    {
        return await _dbSet.Include(x => x.ParentCategory)
            .FirstOrDefaultAsync(x => x.Id == id);
    }
}
