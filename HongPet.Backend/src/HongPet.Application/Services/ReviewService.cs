using AutoMapper;
using HongPet.Application.Services.Abstractions;
using HongPet.Application.Services.Commons;
using HongPet.Domain.Entities;
using HongPet.Domain.Entities.Commons;
using HongPet.Domain.Repositories.Abstractions;
using HongPet.SharedViewModels.Generals;
using HongPet.SharedViewModels.Models;
using HongPet.SharedViewModels.ViewModels;

namespace HongPet.Application.Services;
public class ReviewService : GenericService<Review>, IReviewService
{
    private readonly IReviewRepository _reviewRepository;
    private readonly IOrderRepository _orderRepository;
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;
    public ReviewService(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IClaimService claimService) : base(unitOfWork, claimService)
    {
        _reviewRepository = unitOfWork.ReviewRepository;
        _repository = _reviewRepository;
        _mapper = mapper;
        _orderRepository = unitOfWork.OrderRepository;
        _productRepository = unitOfWork.ProductRepository;
    }

    public async Task<ReviewVM> CreateReviewAsync(ReviewCreateModel reviewModel)
    {
        var review = _mapper.Map<Review>(reviewModel);
        
        // check valid forgein keys
        var currentUserId = _claimService.GetCurrentUserId;
        var order = await _orderRepository.GetByIdAsync(review.OrderId);        

        if (currentUserId == null)
        {
            throw new UnauthorizedAccessException(
                "You must login to use this feature.");
        }

        if (order == null)
        {
            throw new KeyNotFoundException(
                $"Order with the id {review.OrderId} not found.");
        }

        if (order.CustomerId != currentUserId)
        {
            throw new UnauthorizedAccessException(
                $"You are not authorized to create review " +
                $"for the product {review.ProductId} in the order {review.OrderId}.");
        }

        if (!(await _productRepository.IsExistAsync(review.ProductId)))
        {
            throw new KeyNotFoundException(
                $"Product with the id {review.ProductId} not found.");
        }

        // add review
        review.CustomerId = currentUserId.Value;

        await base.AddAsync(review);

        return _mapper.Map<ReviewVM>(review);
    }

    // authorize: own user, admin | soft delete
    public async Task DeleteReviewAsync(Guid reviewId)
    {
        await CheckReviewOwnerAuthorizeAsync(reviewId);

        await base.SoftDeleteAsync(reviewId);
    }

    public async Task<IPagedList<ReviewVM>> GetReviewsByProductIdAsync(Guid productId, 
        int pageIndex = 1, int pageSize = 10)
    {
        var pagedReviews = await _reviewRepository
            .GetReviewsByProductIdAsync(productId, pageIndex, pageSize);

        return _mapper.Map<PagedList<ReviewVM>>(pagedReviews);
    }

    public async Task<ReviewVM> UpdateReviewAsync(
        Guid id, ReviewUpdateModel reviewModel)
    {
        var (existingReview, currentUserId) = await CheckReviewOwnerAuthorizeAsync(id);

        _mapper.Map(reviewModel, existingReview);

        await base.UpdateAsync(existingReview);

        return _mapper.Map<ReviewVM>(existingReview);
    }

    private async Task<(Review, Guid)> CheckReviewOwnerAuthorizeAsync(Guid reviewId)
    {
        var review = await _reviewRepository
                                .GetByIdAsync(reviewId);

        if (review == null || review.DeletedDate != null)
        {
            throw new KeyNotFoundException($"Review with the id {reviewId} not found or has been deleted.");
        }

        var currentUserId = _claimService.GetCurrentUserId;
        if (currentUserId == null)
        {
            throw new UnauthorizedAccessException("You must login to use this feature.");
        }

        if (review.CustomerId != currentUserId && !_claimService.IsAdmin!.Value)
        {
            throw new UnauthorizedAccessException(
                "You are not authorized to delete this review.");
        }

        return (review, currentUserId.Value);
    }
}
