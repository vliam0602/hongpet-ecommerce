using Domain.Entities.Commons;
using HongPet.Application.Commons;
using HongPet.Domain.Entities;
using HongPet.SharedViewModels.Models;
using HongPet.SharedViewModels.ViewModels;

namespace HongPet.Application.Services.Abstractions;
public interface IReviewService : IGenericService<Review>
{
    Task<ReviewVM> CreateReviewAsync(ReviewCreateModel reviewModel);
    Task<ReviewVM> UpdateReviewAsync(Guid id, ReviewUpdateModel reviewModel);
    Task DeleteReviewAsync(Guid reviewId);
    Task<IPagedList<ReviewVM>> GetReviewsByProductIdAsync(Guid productId,
        int pageIndex = 1, int pageSize = 10);
}
