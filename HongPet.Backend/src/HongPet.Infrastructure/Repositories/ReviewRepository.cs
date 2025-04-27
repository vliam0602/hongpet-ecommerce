using HongPet.Domain.Entities;
using HongPet.Domain.Repositories.Abstractions;
using HongPet.Infrastructure.Database;
using HongPet.Infrastructure.Repositories.Commons;
using HongPet.SharedViewModels.Generals;
using Microsoft.EntityFrameworkCore;

namespace HongPet.Infrastructure.Repositories;
public class ReviewRepository : GenericRepository<Review>, IReviewRepository
{
    public ReviewRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<IPagedList<Review>> GetReviewsByProductIdAsync(Guid productId, int pageIndex, int pageSize)
    {
        var reviews = _dbSet
            .Include(x => x.Customer)
            .Where(x => x.ProductId == productId
                        && x.DeletedDate == null);
        return await base.ToPaginationAsync(reviews, pageIndex, pageSize);
    }
}
