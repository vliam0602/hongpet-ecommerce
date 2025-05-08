using AutoFixture;
using FluentAssertions;
using HongPet.Domain.Entities;
using HongPet.Domain.Test;
using HongPet.Infrastructure.Database;
using HongPet.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace HongPet.Infrastructure.Test.Repositories;

public class OrderRepositoryTest : SetupTest
{
    private readonly OrderRepository _orderRepository;
    private readonly AppDbContext _context;

    public OrderRepositoryTest()
    {
        _context = new AppDbContext(new DbContextOptionsBuilder<AppDbContext>()
                    .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                    .Options);

        // Reset the database
        _context.Orders.RemoveRange(_context.Orders);
        _context.SaveChanges();

        // Initialize the order repository
        _orderRepository = new OrderRepository(_context, _mapper);
    }

    [Fact]
    public async Task GetOrderByCustomerIdAsync_ShouldReturnPagedOrders_WhenDataExists()
    {
        // Arrange
        var customerId = Guid.NewGuid();
        var orders = _fixture.Build<Order>()
                       .With(o => o.CustomerId, customerId)
                       .Without(o => o.OrderItems)
                       .Without(o => o.Reviews)
                       .Without(o => o.Customer)
                       .Without(o => o.DeletedDate)
                       .Without(o => o.DeletedBy)
                       .CreateMany(5)
                       .ToList();

        _context.Orders.AddRange(orders);
        await _context.SaveChangesAsync();

        // Act
        var result = await _orderRepository.GetOrderByCustomerIdAsync(
            customerId, pageIndex: 1, pageSize: 2);

        // Assert
        result.Should().NotBeNull();
        result.Items.Should().HaveCount(2);
        result.Items.First().CustomerId.Should().Be(customerId);
        result.TotalCount.Should().Be(5);
        result.CurrentPage.Should().Be(1);
        result.TotalPages.Should().Be(3);
    }

    [Fact]
    public async Task GetOrderByCustomerIdAsync_ShouldReturnEmpty_WhenCustomerHasNoOrders()
    {
        // Arrange
        var customerId = Guid.NewGuid();

        // Act
        var result = await _orderRepository.GetOrderByCustomerIdAsync(
            customerId, pageIndex: 1, pageSize: 10);

        // Assert
        result.Should().NotBeNull();
        result.Items.Should().BeEmpty();
        result.TotalCount.Should().Be(0);
    }

    [Fact]
    public async Task GetOrderDetailAsync_ShouldReturnOrderDetail_WhenOrderExists()
    {
        // Arrange        
        var order = _fixture.Build<Order>()
                            .Without(o => o.OrderItems)
                            .Without(o => o.Reviews)
                            .Without(o => o.Customer)
                            .Without(o => o.DeletedDate)
                            .Without(o => o.DeletedBy)
                            .Create();

        _context.Orders.Add(order);
        await _context.SaveChangesAsync();

        // Act
        var result = await _orderRepository.GetOrderDetailAsync(order.Id);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(order.Id);
    }

    [Fact]
    public async Task GetOrderDetailAsync_ShouldReturnNull_WhenOrderDoesNotExist()
    {
        // Arrange
        var nonExistentOrderId = Guid.NewGuid();

        // Act
        var result = await _orderRepository.GetOrderDetailAsync(nonExistentOrderId);

        // Assert
        result.Should().BeNull();
    }
}
