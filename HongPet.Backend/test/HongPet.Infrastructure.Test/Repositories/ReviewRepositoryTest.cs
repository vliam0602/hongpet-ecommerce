using AutoFixture;
using FluentAssertions;
using HongPet.Domain.Entities;
using HongPet.Domain.Test;
using HongPet.Infrastructure.Database;
using HongPet.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace HongPet.Infrastructure.Test.Repositories;

public class ReviewRepositoryTest : SetupTest
{
    private readonly ReviewRepository _reviewRepository;
    private readonly AppDbContext _context;

    public ReviewRepositoryTest()
    {
        _context = new AppDbContext(new DbContextOptionsBuilder<AppDbContext>()
                    .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                    .Options);

        // Reset the database
        _context.Reviews.RemoveRange(_context.Reviews);
        _context.SaveChanges();

        // Initialize the review repository
        _reviewRepository = new ReviewRepository(_context);
    }

    [Fact]
    public async Task GetReviewsByProductIdAsync_ShouldReturnPagedReviews_WhenDataExists()
    {
        // Arrange
        var productId = Guid.NewGuid();
        var customer = _fixture.Build<User>()
                               .Without(u => u.Orders)
                               .Without(u => u.Reviews)
                               .Create();

        var reviews = _fixture.Build<Review>()
                              .With(r => r.ProductId, productId)
                              .With(r => r.Customer, customer)
                              .With(r => r.CustomerId, customer.Id)
                              .Without(r => r.Order)
                              .Without(r => r.Product)
                              .With(r => r.DeletedDate, () => null)
                              .Without(r => r.DeletedBy)
                              .CreateMany(5)
                              .ToList();

        _context.Reviews.AddRange(reviews);
        await _context.SaveChangesAsync();

        // Act
        var result = await _reviewRepository.GetReviewsByProductIdAsync(
            productId, pageIndex: 1, pageSize: 2);

        // Assert
        result.Should().NotBeNull();
        result.Items.Should().HaveCount(2);
        result.TotalCount.Should().Be(reviews.Count);
        result.CurrentPage.Should().Be(1);
        result.TotalPages.Should().Be(3);
    }

    [Fact]
    public async Task GetReviewsByProductIdAsync_ShouldReturnEmpty_WhenNoReviewsExistForProduct()
    {
        // Arrange
        var productId = Guid.NewGuid();

        // Act
        var result = await _reviewRepository.GetReviewsByProductIdAsync(productId, pageIndex: 1, pageSize: 10);

        // Assert
        result.Should().NotBeNull();
        result.Items.Should().BeEmpty();
        result.TotalCount.Should().Be(0);
    }

    [Fact]
    public async Task GetReviewsByProductIdAsync_ShouldExcludeDeletedReviews()
    {
        // Arrange
        var productId = Guid.NewGuid();

        var customer = _fixture.Build<User>()
                               .Without(u => u.Orders)
                               .Without(u => u.Reviews)
                               .Create();
        var reviews = _fixture.Build<Review>()
                              .With(r => r.ProductId, productId)
                              .With(r => r.CustomerId, customer.Id)
                              .With(r => r.Customer, customer)
                              .Without(r => r.DeletedDate)                              
                              .Without(r => r.DeletedBy)                           
                              .Without(r => r.Order)
                              .Without(r => r.Product)
                              .CreateMany(6)
                              .ToList();

        reviews[0].DeletedDate = DateTime.UtcNow;

        _context.Reviews.AddRange(reviews);
        await _context.SaveChangesAsync();

        // Act
        var result = await _reviewRepository.GetReviewsByProductIdAsync(
            productId, pageIndex: 1, pageSize: 10);

        // Assert
        result.Should().NotBeNull();
        result.Items.Should().HaveCount(5);
        result.Items.Should().NotContain(r => r.DeletedDate != null);
    }
}
