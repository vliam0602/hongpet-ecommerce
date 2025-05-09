using AutoFixture;
using FluentAssertions;
using HongPet.Application.Services;
using HongPet.Domain.Entities;
using HongPet.Domain.Repositories.Abstractions;
using HongPet.Domain.Test;
using HongPet.SharedViewModels.Generals;
using HongPet.SharedViewModels.Models;
using HongPet.SharedViewModels.ViewModels;
using Moq;

namespace HongPet.Application.Test.Services;

public class ReviewServiceTest : SetupTest
{
    private readonly ReviewService _reviewService;
    private readonly Mock<IReviewRepository> _reviewRepositoryMock;
    private readonly Mock<IOrderRepository> _orderRepositoryMock;
    private readonly Mock<IProductRepository> _productRepositoryMock;

    public ReviewServiceTest()
    {
        // Mock the ReviewRepository, OrderRepository, and ProductRepository
        _reviewRepositoryMock = new Mock<IReviewRepository>();
        _orderRepositoryMock = new Mock<IOrderRepository>();
        _productRepositoryMock = new Mock<IProductRepository>();

        _unitOfWorkMock.SetupGet(x => x.ReviewRepository).Returns(_reviewRepositoryMock.Object);
        _unitOfWorkMock.SetupGet(x => x.OrderRepository).Returns(_orderRepositoryMock.Object);
        _unitOfWorkMock.SetupGet(x => x.ProductRepository).Returns(_productRepositoryMock.Object);

        // Initialize the service with mocked dependencies
        _reviewService = new ReviewService(
            _unitOfWorkMock.Object,
            _mapper,
            _claimServiceMock.Object);
    }

    [Fact]
    public async Task CreateReviewAsync_ShouldCreateReview_WhenValid()
    {
        // Arrange  
        var reviewModel = _fixture.Build<ReviewCreateModel>().Create();
        var currentUserId = Guid.NewGuid();
        var mockOrder = _fixture.Build<Order>()
                                .With(o => o.Id, reviewModel.OrderId)
                                .With(o => o.CustomerId, currentUserId)
                                .Create();

        _claimServiceMock.Setup(x => x.GetCurrentUserId).Returns(currentUserId);
        _orderRepositoryMock.Setup(x => x.GetByIdAsync(reviewModel.OrderId)).ReturnsAsync(mockOrder);
        _productRepositoryMock.Setup(x => x.IsExistAsync(reviewModel.ProductId)).ReturnsAsync(true);

        // Act  
        var result = await _reviewService.CreateReviewAsync(reviewModel);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.Id != Guid.Empty);
        _reviewRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Review>()), Times.Once);
        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task CreateReviewAsync_ShouldThrowUnauthorizedAccessException_WhenUserNotLoggedIn()
    {
        // Arrange
        var reviewModel = _fixture.Build<ReviewCreateModel>().Create();

        _claimServiceMock.Setup(x => x.GetCurrentUserId).Returns(() => null);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<UnauthorizedAccessException>(() =>
            _reviewService.CreateReviewAsync(reviewModel));
        Assert.Equal("You must login to use this feature.", exception.Message);
    }

    [Fact]
    public async Task CreateReviewAsync_ShouldThrowUnauthorizedAccessException_WhenUserUnauthorized()
    {
        // Arrange
        var reviewModel = _fixture.Build<ReviewCreateModel>().Create();
        var mockOrder = _fixture.Build<Order>()
                                .With(o => o.Id, reviewModel.OrderId)
                                .Create();

        _claimServiceMock.Setup(x => x.GetCurrentUserId).Returns(() => Guid.NewGuid());
        _orderRepositoryMock.Setup(x => x.GetByIdAsync(reviewModel.OrderId)).ReturnsAsync(mockOrder);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<UnauthorizedAccessException>(() =>
            _reviewService.CreateReviewAsync(reviewModel));
        Assert.Contains("You are not authorized to create review", exception.Message);
    }

    [Fact]
    public async Task CreateReviewAsync_ShouldThrowKeyNotFoundException_WhenOrderNotFound()
    {
        // Arrange
        var reviewModel = _fixture.Build<ReviewCreateModel>().Create();
        var currentUserId = Guid.NewGuid();

        _claimServiceMock.Setup(x => x.GetCurrentUserId).Returns(currentUserId);
        _orderRepositoryMock.Setup(x => x.GetByIdAsync(reviewModel.OrderId)).ReturnsAsync((Order?)null);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() =>
            _reviewService.CreateReviewAsync(reviewModel));
        exception.Message.Should().BeEquivalentTo($"Order with the id {reviewModel.OrderId} not found.");
        _orderRepositoryMock.Verify(x => x.GetByIdAsync(reviewModel.OrderId), Times.Once);        
        _reviewRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Review>()), Times.Never);
        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(), Times.Never);
    }

    [Fact]
    public async Task CreateReviewAsync_ShouldThrowKeyNotFoundException_WhenProductNotFound()
    {
        // Arrange
        var reviewModel = _fixture.Build<ReviewCreateModel>().Create();
        var currentUserId = Guid.NewGuid();
        var mockOrder = _fixture.Build<Order>()
                                .With(o => o.Id, reviewModel.OrderId)
                                .With(o => o.CustomerId, currentUserId)
                                .Create();

        _claimServiceMock.Setup(x => x.GetCurrentUserId).Returns(currentUserId);
        _orderRepositoryMock.Setup(x => x.GetByIdAsync(reviewModel.OrderId)).ReturnsAsync(mockOrder);
        _productRepositoryMock.Setup(x => x.IsExistAsync(reviewModel.ProductId)).ReturnsAsync(false);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() =>
            _reviewService.CreateReviewAsync(reviewModel));        
        exception.Message.Should().BeEquivalentTo($"Product with the id {reviewModel.ProductId} not found.");
        _orderRepositoryMock.Verify(x => x.GetByIdAsync(reviewModel.OrderId), Times.Once);
        _productRepositoryMock.Verify(x => x.IsExistAsync(reviewModel.ProductId), Times.Once);
        _reviewRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Review>()), Times.Never);
        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(), Times.Never);
    }


    [Fact]
    public async Task DeleteReviewAsync_ShouldSoftDeleteReview_WhenAuthorized()
    {
        // Arrange
        var reviewId = Guid.NewGuid();
        var mockReview = _fixture.Build<Review>()
                                 .With(r => r.Id, reviewId)
                                 .With(r =>r.DeletedDate, () => null)
                                 .Without(r =>r.DeletedBy)
                                 .Create();
        var currentUserId = mockReview.CustomerId;

        _claimServiceMock.Setup(x => x.GetCurrentUserId).Returns(currentUserId);
        _reviewRepositoryMock.Setup(x => x.GetByIdAsync(reviewId)).ReturnsAsync(mockReview);

        // Act
        await _reviewService.DeleteReviewAsync(reviewId);

        // Assert
        _reviewRepositoryMock.Verify(x => x.Update(It.IsAny<Review>()), Times.Once);
        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(), Times.Once);
        mockReview.DeletedDate.Should().NotBeNull();
    }

    [Fact]
    public async Task DeleteReviewAsync_ShouldThrowKeyNotFoundException_WhenReviewNotFound()
    {
        // Arrange
        var reviewId = Guid.NewGuid();
        _reviewRepositoryMock.Setup(x => x.GetByIdAsync(reviewId)).ReturnsAsync(() => null);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() =>
            _reviewService.DeleteReviewAsync(reviewId));
    }

    [Fact]
    public async Task DeleteReviewAsync_ShouldThrowUnauthorizedAccessException_WhenNotLogIn()
    {
        // Arrange
        var reviewId = Guid.NewGuid();
        var mockReview = _fixture.Build<Review>()
                                 .With(r => r.Id, reviewId)
                                 .With(r => r.DeletedDate, () => null)
                                 .Without(r => r.DeletedBy)
                                 .Create();

        _claimServiceMock.SetupGet(x => x.GetCurrentUserId).Returns((Guid?)null);
        _reviewRepositoryMock.Setup(x => x.GetByIdAsync(reviewId)).ReturnsAsync(mockReview);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<UnauthorizedAccessException>(() =>
            _reviewService.DeleteReviewAsync(reviewId));
        Assert.Equal("You must login to use this feature.", exception.Message);
    }

    [Fact]
    public async Task DeleteReviewAsync_ShouldThrowUnauthorizedAccessException_WhenNotAuthorized()
    {
        // Arrange
        var reviewId = Guid.NewGuid();
        var mockReview = _fixture.Build<Review>()
                                 .With(r => r.Id, reviewId)
                                 .With(r => r.DeletedDate, () => null)
                                 .Without(r => r.DeletedBy)
                                 .Create();

        _claimServiceMock.SetupGet(x => x.GetCurrentUserId).Returns(Guid.NewGuid());
        _claimServiceMock.SetupGet(x => x.IsAdmin).Returns(false);
        _reviewRepositoryMock.Setup(x => x.GetByIdAsync(reviewId)).ReturnsAsync(mockReview);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<UnauthorizedAccessException>(() =>
            _reviewService.DeleteReviewAsync(reviewId));
        Assert.Equal("You are not authorized to delete this review.", exception.Message);
    }

    [Fact]
    public async Task GetReviewsByProductIdAsync_ShouldReturnPagedReviews()
    {
        // Arrange
        var productId = Guid.NewGuid();
        var mockReviews = _fixture.Build<Review>()
                                  .With(x => x.ProductId, productId)
                                  .CreateMany(5).ToList();
        var pagedReviews = new PagedList<Review>(mockReviews, 5, 1, 10);

        _reviewRepositoryMock.Setup(x => x.GetReviewsByProductIdAsync(productId, 1, 10))
                             .ReturnsAsync(pagedReviews);

        // Act
        var result = await _reviewService.GetReviewsByProductIdAsync(productId);

        // Assert
        var expectedOutput = _mapper.Map<PagedList<ReviewVM>>(pagedReviews);
        Assert.NotNull(result);
        Assert.Equal(5, result.Items.Count);
        result.Should().BeEquivalentTo(expectedOutput);
        _reviewRepositoryMock.Verify(x => x.GetReviewsByProductIdAsync(productId, 1, 10), Times.Once);
    }

    [Fact]
    public async Task UpdateReviewAsync_ShouldUpdateReview_WhenAuthorized()
    {
        // Arrange
        var reviewId = Guid.NewGuid();
        var reviewModel = _fixture.Build<ReviewUpdateModel>().Create();
        var mockReview = _fixture.Build<Review>()
                                 .With(r => r.Id, reviewId)
                                 .Without(r => r.DeletedDate)
                                 .Without(r => r.DeletedBy)
                                 .Create();
        var currentUserId = mockReview.CustomerId;

        _claimServiceMock.Setup(x => x.GetCurrentUserId).Returns(currentUserId);
        _reviewRepositoryMock.Setup(x => x.GetByIdAsync(reviewId)).ReturnsAsync(mockReview);

        // Act
        var result = await _reviewService.UpdateReviewAsync(reviewId, reviewModel);

        // Assert        
        Assert.NotNull(result);
        Assert.Equal(reviewId, result.Id);
        _reviewRepositoryMock.Verify(x => x.Update(It.IsAny<Review>()), Times.Once);
        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task UpdateReviewAsync_ShouldThrowUnauthorizedAccessException_WhenNotAuthorized()
    {
        // Arrange
        var reviewId = Guid.NewGuid();
        var reviewModel = _fixture.Build<ReviewUpdateModel>().Create();
        var mockReview = _fixture.Build<Review>()
                                 .With(r => r.Id, reviewId)
                                 .Without(r => r.DeletedDate)
                                 .Without(r => r.DeletedBy)
                                 .Create();

        _claimServiceMock.Setup(x => x.GetCurrentUserId).Returns(Guid.NewGuid());
        _claimServiceMock.Setup(x => x.IsAdmin).Returns(false);
        _reviewRepositoryMock.Setup(x => x.GetByIdAsync(reviewId)).ReturnsAsync(mockReview);

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() =>
            _reviewService.UpdateReviewAsync(reviewId, reviewModel));
    }
}
