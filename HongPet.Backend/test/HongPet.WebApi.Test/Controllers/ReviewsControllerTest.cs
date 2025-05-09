using AutoFixture;
using FluentAssertions;
using HongPet.Application.Services.Abstractions;
using HongPet.Domain.Entities;
using HongPet.Domain.Test;
using HongPet.SharedViewModels.Models;
using HongPet.SharedViewModels.ViewModels;
using HongPet.WebApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace HongPet.WebApi.Test.Controllers;

public class ReviewsControllerTest : SetupTest
{
    private readonly ReviewsController _reviewsController;
    private readonly Mock<IReviewService> _reviewServiceMock;

    public ReviewsControllerTest()
    {
        // Mock IReviewService
        _reviewServiceMock = _fixture.Freeze<Mock<IReviewService>>();

        // Initialize ReviewsController
        _reviewsController = new ReviewsController(_reviewServiceMock.Object);
    }

    [Fact]
    public async Task CreateReview_ShouldReturnCreated_WhenReviewIsCreated()
    {
        // Arrange
        var reviewModel = _fixture.Create<ReviewCreateModel>();
        var createdReview = _fixture.Create<ReviewVM>();
        _reviewServiceMock.Setup(s => s.CreateReviewAsync(reviewModel))
                          .ReturnsAsync(createdReview);

        // Act
        var result = await _reviewsController.CreateReview(reviewModel);

        // Assert
        result.Should().BeOfType<CreatedAtActionResult>();
        var createdResult = result as CreatedAtActionResult;
        createdResult.Should().NotBeNull();
        createdResult!.StatusCode.Should().Be(201); // Status code 201
        var response = createdResult.Value as ApiResponse;
        response.Should().NotBeNull();
        response!.IsSuccess.Should().BeTrue();
        response.Data.Should().Be(createdReview);
    }

    [Fact]
    public async Task CreateReview_ShouldReturnUnauthorized_WhenUnauthorizedAccessExceptionIsThrown()
    {
        // Arrange
        var reviewModel = _fixture.Create<ReviewCreateModel>();
        _reviewServiceMock.Setup(s => s.CreateReviewAsync(reviewModel))
                          .ThrowsAsync(new UnauthorizedAccessException("Unauthorized access"));

        // Act
        var result = await _reviewsController.CreateReview(reviewModel);

        // Assert
        result.Should().BeOfType<UnauthorizedObjectResult>();
        var unauthorizedResult = result as UnauthorizedObjectResult;
        unauthorizedResult.Should().NotBeNull();
        unauthorizedResult!.StatusCode.Should().Be(401); // Status code 401
        var response = unauthorizedResult.Value as ApiResponse;
        response.Should().NotBeNull();
        response!.IsSuccess.Should().BeFalse();
        response.ErrorMessage.Should().Be("Unauthorized access");
    }

    [Fact]
    public async Task CreateReview_ShouldReturnNotFound_WhenKeyNotFoundExceptionIsThrown()
    {
        // Arrange
        var reviewModel = _fixture.Create<ReviewCreateModel>();
        _reviewServiceMock.Setup(s => s.CreateReviewAsync(reviewModel))
                          .ThrowsAsync(new KeyNotFoundException("Order not found"));

        // Act
        var result = await _reviewsController.CreateReview(reviewModel);

        // Assert
        result.Should().BeOfType<NotFoundObjectResult>();
        var notFoundResult = result as NotFoundObjectResult;
        notFoundResult.Should().NotBeNull();
        notFoundResult!.StatusCode.Should().Be(404); // Status code 404
        var response = notFoundResult.Value as ApiResponse;
        response.Should().NotBeNull();
        response!.IsSuccess.Should().BeFalse();
        response.ErrorMessage.Should().Be("Order not found");
    }

    [Fact]
    public async Task CreateReview_ShouldReturnInternalServerError_WhenExceptionIsThrown()
    {
        // Arrange
        var reviewModel = _fixture.Create<ReviewCreateModel>();
        _reviewServiceMock.Setup(s => s.CreateReviewAsync(reviewModel))
                          .ThrowsAsync(new Exception("Unexpected error"));

        // Act
        var result = await _reviewsController.CreateReview(reviewModel);

        // Assert
        result.Should().BeOfType<ObjectResult>();
        var objectResult = result as ObjectResult;
        objectResult.Should().NotBeNull();
        objectResult!.StatusCode.Should().Be(500); // Status code 500
        var response = objectResult.Value as ApiResponse;
        response.Should().NotBeNull();
        response!.IsSuccess.Should().BeFalse();
        response.ErrorMessage.Should().Contain("Unexpected Error");
    }

    [Fact]
    public async Task UpdateReview_ShouldReturnOk_WhenReviewIsUpdated()
    {
        // Arrange
        var reviewId = Guid.NewGuid();
        var reviewModel = _fixture.Create<ReviewUpdateModel>();
        var updatedReview = _fixture.Create<ReviewVM>();
        _reviewServiceMock.Setup(s => s.UpdateReviewAsync(reviewId, reviewModel)).ReturnsAsync(updatedReview);

        // Act
        var result = await _reviewsController.UpdateReview(reviewId, reviewModel);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = result as OkObjectResult;
        okResult.Should().NotBeNull();
        okResult!.StatusCode.Should().Be(200); // Status code 200
        var response = okResult.Value as ApiResponse;
        response.Should().NotBeNull();
        response!.IsSuccess.Should().BeTrue();
        response.Message.Should().Contain(reviewId.ToString());
        response.Data.Should().Be(updatedReview);
    }

    [Fact]
    public async Task UpdateReview_ShouldReturnUnauthorized_WhenUnauthorizedAccessExceptionIsThrown()
    {
        // Arrange
        var reviewId = Guid.NewGuid();
        var reviewModel = _fixture.Create<ReviewUpdateModel>();
        _reviewServiceMock.Setup(s => s.UpdateReviewAsync(reviewId, reviewModel))
                          .ThrowsAsync(new UnauthorizedAccessException("Unauthorized access"));

        // Act
        var result = await _reviewsController.UpdateReview(reviewId, reviewModel);

        // Assert
        result.Should().BeOfType<UnauthorizedObjectResult>();
        var unauthorizedResult = result as UnauthorizedObjectResult;
        unauthorizedResult.Should().NotBeNull();
        unauthorizedResult!.StatusCode.Should().Be(401); // Status code 401
        var response = unauthorizedResult.Value as ApiResponse;
        response.Should().NotBeNull();
        response!.IsSuccess.Should().BeFalse();
        response.ErrorMessage.Should().Be("Unauthorized access");
    }

    [Fact]
    public async Task UpdateReview_ShouldReturnInternalServerError_WhenExceptionIsThrown()
    {
        // Arrange
        var reviewId = Guid.NewGuid();
        var reviewModel = _fixture.Create<ReviewUpdateModel>();
        _reviewServiceMock.Setup(s => s.UpdateReviewAsync(reviewId, reviewModel))
                          .ThrowsAsync(new Exception("Unexpected error"));

        // Act
        var result = await _reviewsController.UpdateReview(reviewId, reviewModel);

        // Assert
        result.Should().BeOfType<ObjectResult>();
        var objectResult = result as ObjectResult;
        objectResult.Should().NotBeNull();
        objectResult!.StatusCode.Should().Be(500); // Status code 500
        var response = objectResult.Value as ApiResponse;
        response.Should().NotBeNull();
        response!.IsSuccess.Should().BeFalse();
        response.ErrorMessage.Should().Contain("Unexpected Error");
    }

    [Fact]
    public async Task SoftDeleteReview_ShouldReturnOk_WhenReviewIsDeleted()
    {
        // Arrange
        var reviewId = Guid.NewGuid();
        _reviewServiceMock.Setup(s => s.DeleteReviewAsync(reviewId)).Returns(Task.CompletedTask);

        // Act
        var result = await _reviewsController.SoftDeleteReview(reviewId);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = result as OkObjectResult;
        okResult.Should().NotBeNull();
        okResult!.StatusCode.Should().Be(200); // Status code 200
        var response = okResult.Value as ApiResponse;
        response.Should().NotBeNull();
        response!.IsSuccess.Should().BeTrue();
        response.Message.Should().Contain(reviewId.ToString());
    }

    [Fact]
    public async Task SoftDeleteReview_ShouldReturnNotFound_WhenReviewDoesNotExist()
    {
        // Arrange
        var reviewId = Guid.NewGuid();
        _reviewServiceMock.Setup(s => s.DeleteReviewAsync(reviewId))
                          .ThrowsAsync(new KeyNotFoundException("Review not found"));

        // Act
        var result = await _reviewsController.SoftDeleteReview(reviewId);

        // Assert
        result.Should().BeOfType<NotFoundObjectResult>();
        var notFoundResult = result as NotFoundObjectResult;
        notFoundResult.Should().NotBeNull();
        notFoundResult!.StatusCode.Should().Be(404); // Status code 404
        var response = notFoundResult.Value as ApiResponse;
        response.Should().NotBeNull();
        response!.IsSuccess.Should().BeFalse();
        response.ErrorMessage.Should().Contain("Review not found");
    }

    [Fact]
    public async Task SoftDeleteReview_ShouldReturnBadRequest_WhenArgumentExceptionIsThrown()
    {
        // Arrange
        var reviewId = Guid.NewGuid();
        var exceptionMessage = "Invalid review ID";
        _reviewServiceMock.Setup(s => s.DeleteReviewAsync(reviewId))
                          .ThrowsAsync(new ArgumentException(exceptionMessage));

        // Act
        var result = await _reviewsController.SoftDeleteReview(reviewId);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
        var badRequestResult = result as BadRequestObjectResult;
        badRequestResult.Should().NotBeNull();
        badRequestResult!.StatusCode.Should().Be(400); // Status code 400
        var response = badRequestResult.Value as ApiResponse;
        response.Should().NotBeNull();
        response!.IsSuccess.Should().BeFalse();
        response.ErrorMessage.Should().Be(exceptionMessage);
    }

    [Fact]
    public async Task SoftDeleteReview_ShouldReturnUnauthorized_WhenUnauthorizedAccessExceptionIsThrown()
    {
        // Arrange
        var reviewId = Guid.NewGuid();
        var exceptionMessage = "You are not authorized to delete this review";
        _reviewServiceMock.Setup(s => s.DeleteReviewAsync(reviewId))
                          .ThrowsAsync(new UnauthorizedAccessException(exceptionMessage));

        // Act
        var result = await _reviewsController.SoftDeleteReview(reviewId);

        // Assert
        result.Should().BeOfType<UnauthorizedObjectResult>();
        var unauthorizedResult = result as UnauthorizedObjectResult;
        unauthorizedResult.Should().NotBeNull();
        unauthorizedResult!.StatusCode.Should().Be(401); // Status code 401
        var response = unauthorizedResult.Value as ApiResponse;
        response.Should().NotBeNull();
        response!.IsSuccess.Should().BeFalse();
        response.ErrorMessage.Should().Be(exceptionMessage);
    }


    [Fact]
    public async Task SoftDeleteReview_ShouldReturnInternalServerError_WhenExceptionIsThrown()
    {
        // Arrange
        var reviewId = Guid.NewGuid();
        _reviewServiceMock.Setup(s => s.DeleteReviewAsync(reviewId))
                          .ThrowsAsync(new Exception("Unexpected error"));

        // Act
        var result = await _reviewsController.SoftDeleteReview(reviewId);

        // Assert
        result.Should().BeOfType<ObjectResult>();
        var objectResult = result as ObjectResult;
        objectResult.Should().NotBeNull();
        objectResult!.StatusCode.Should().Be(500); // Status code 500
        var response = objectResult.Value as ApiResponse;
        response.Should().NotBeNull();
        response!.IsSuccess.Should().BeFalse();
        response.ErrorMessage.Should().Contain("Unexpected Error");
    }

    [Fact]
    public async Task GetReviewById_ShouldReturnOk_WhenReviewExists()
    {
        // Arrange
        var reviewId = Guid.NewGuid();
        var review = _fixture.Create<Review>();
        _reviewServiceMock.Setup(s => s.GetByIdAsync(reviewId)).ReturnsAsync(review);

        // Act
        var result = await _reviewsController.GetReviewById(reviewId);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = result as OkObjectResult;
        okResult.Should().NotBeNull();
        okResult!.StatusCode.Should().Be(200); // Status code 200
        var response = okResult.Value as ApiResponse;
        response.Should().NotBeNull();
        response!.IsSuccess.Should().BeTrue();
        response.Data.Should().Be(review);
    }

    [Fact]
    public async Task GetReviewById_ShouldReturnNotFound_WhenReviewDoesNotExist()
    {
        // Arrange
        var reviewId = Guid.NewGuid();
        _reviewServiceMock.Setup(s => s.GetByIdAsync(reviewId)).ReturnsAsync((Review?)null);

        // Act
        var result = await _reviewsController.GetReviewById(reviewId);

        // Assert
        result.Should().BeOfType<NotFoundObjectResult>();
        var notFoundResult = result as NotFoundObjectResult;
        notFoundResult.Should().NotBeNull();
        notFoundResult!.StatusCode.Should().Be(404); // Status code 404
        var response = notFoundResult.Value as ApiResponse;
        response.Should().NotBeNull();
        response!.IsSuccess.Should().BeFalse();
        response.ErrorMessage.Should().Contain(reviewId.ToString());
    }

    [Fact]
    public async Task GetReviewById_ShouldReturnInternalServerError_WhenExceptionIsThrown()
    {
        // Arrange
        var reviewId = Guid.NewGuid();
        _reviewServiceMock.Setup(s => s.GetByIdAsync(reviewId))
                          .ThrowsAsync(new Exception("Unexpected error"));

        // Act
        var result = await _reviewsController.GetReviewById(reviewId);

        // Assert
        result.Should().BeOfType<ObjectResult>();
        var objectResult = result as ObjectResult;
        objectResult.Should().NotBeNull();
        objectResult!.StatusCode.Should().Be(500); // Status code 500
        var response = objectResult.Value as ApiResponse;
        response.Should().NotBeNull();
        response!.IsSuccess.Should().BeFalse();
        response.ErrorMessage.Should().Contain("Unexpected Error");
    }
}
