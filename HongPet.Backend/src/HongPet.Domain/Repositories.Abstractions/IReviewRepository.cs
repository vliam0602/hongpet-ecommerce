using HongPet.Domain.Entities;
using HongPet.Domain.Entities.Commons;
using HongPet.SharedViewModels.Generals;

namespace HongPet.Domain.Repositories.Abstractions;
public interface IReviewRepository : IGenericRepository<Review>
{
    Task<IPagedList<Review>> GetReviewsByProductIdAsync(Guid productId, int pageIndex, int pageSize);
}
