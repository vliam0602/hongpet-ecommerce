using Domain.Entities.Commons;
using HongPet.Domain.Entities;

namespace HongPet.Domain.Repositories.Abstractions;
public interface IReviewRepository : IGenericRepository<Review>
{
    Task<IPagedList<Review>> GetReviewsByProductIdAsync(Guid productId, int pageIndex, int pageSize);
}
