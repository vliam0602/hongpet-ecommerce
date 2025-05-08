using AutoFixture;
using Castle.Core.Resource;
using FluentAssertions;
using HongPet.Application.Services;
using HongPet.Domain.Entities;
using HongPet.Domain.Enums;
using HongPet.Domain.Repositories.Abstractions;
using HongPet.Domain.Test;
using HongPet.Infrastructure.DTOs;
using HongPet.SharedViewModels.Generals;
using HongPet.SharedViewModels.Models;
using HongPet.SharedViewModels.ViewModels;
using Moq;

namespace HongPet.Application.Test.Services;

public class OrderServiceTest : SetupTest
{
    private readonly OrderService _orderService;
    private readonly Mock<IOrderRepository> _orderRepoMock;
    private readonly Mock<IGenericRepository<Variant>> _variantRepoMock;

    public OrderServiceTest()
    {
        // Mock the OrderRepository & setup in the UnitOfWork
        _orderRepoMock = new Mock<IOrderRepository>();
        _variantRepoMock = new Mock<IGenericRepository<Variant>>();

        _unitOfWorkMock.SetupGet(x => x.OrderRepository).Returns(_orderRepoMock.Object);
        _unitOfWorkMock.Setup(x => x.Repository<Variant>()).Returns(_variantRepoMock.Object);

        // Initialize the service with mocked dependencies
        _orderService = new OrderService(
            _unitOfWorkMock.Object,
            _mapper,
            _claimServiceMock.Object);
    }

    [Fact]
    public async Task GetOrdersByCustomerIdAsync_ShouldReturnOrders_WhenAuthorized()
    {
        // Arrange
        var customerId = Guid.NewGuid();
        var mockOrders = _fixture.Build<OrderDto>()
                                .With(x => x.CustomerId, customerId)
                                .Without(x => x.OrderItems)
                                .CreateMany(2);
        var pagedOrders = new PagedList<OrderDto>(mockOrders, 2, 1, 10);

        _claimServiceMock.Setup(x => x.GetCurrentUserId).Returns(customerId);
        _claimServiceMock.Setup(x => x.IsAdmin).Returns(false);

        _orderRepoMock.Setup(x => x.GetOrderByCustomerIdAsync(customerId, 1, 10, ""))
                      .ReturnsAsync(pagedOrders);

        // Act
        var result = await _orderService.GetOrdersByCustomerIdAsync(customerId);

        // Assert
        var expectedOutput = _mapper.Map<PagedList<OrderVM>>(pagedOrders);

        Assert.NotNull(result);
        result.Should().BeEquivalentTo(expectedOutput);
        _orderRepoMock.Verify(x => 
            x.GetOrderByCustomerIdAsync(customerId, 1, 10, ""), Times.Once);
    }

    [Theory]
    [InlineData("11111111-1111-1111-1111-111111111111")]
    [InlineData(null)]
    public async Task GetOrdersByCustomerIdAsync_ShouldThrowUnauthorizedAccessException_WhenNotAuthorized(
        string? currentUserId)
    {
        // Arrange
        var customerId = Guid.NewGuid();

        _claimServiceMock.Setup(x => x.GetCurrentUserId)
            .Returns(currentUserId == null ? null : Guid.Parse(currentUserId));

        _claimServiceMock.Setup(x => x.IsAdmin).Returns(false);

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() =>
            _orderService.GetOrdersByCustomerIdAsync(customerId));
    }

    [Fact]
    public async Task GetOrderWithDetailsAsync_ShouldReturnOrderDetails_WhenAuthorized()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        var mockOrderItems = _fixture.Build<OrderItemDto>()                        
                                    .CreateMany(2);
        var mockOrder = _fixture.Build<OrderDto>()
                                .With(o => o.Id, orderId)
                                .Create();

        _claimServiceMock.Setup(x => x.GetCurrentUserId).Returns(mockOrder.CustomerId);
        _claimServiceMock.Setup(x => x.IsAdmin).Returns(false);

        _orderRepoMock.Setup(x => x.GetOrderDetailAsync(orderId)).ReturnsAsync(mockOrder);

        // Act
        var result = await _orderService.GetOrderWithDetailsAsync(orderId);

        // Assert
        var expectedOutput = _mapper.Map<OrderVM>(mockOrder);
        Assert.NotNull(result);
        Assert.Equal(orderId, result.Id);
        result.Should().BeEquivalentTo(expectedOutput);
        _orderRepoMock.Verify(x => x.GetOrderDetailAsync(orderId), Times.Once);
    }

    [Fact]
    public async Task GetOrderWithDetailsAsync_ShouldThrowKeyNotFoundException_WhenOrderNotFound()
    {
        // Arrange
        var orderId = Guid.NewGuid();

        _claimServiceMock.Setup(x => x.GetCurrentUserId).Returns(Guid.NewGuid());
        _claimServiceMock.Setup(x => x.IsAdmin).Returns(true);

        _orderRepoMock.Setup(x => x.GetOrderDetailAsync(orderId)).ReturnsAsync((OrderDto?)null);

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() =>
            _orderService.GetOrderWithDetailsAsync(orderId));
    }

    [Theory]
    [InlineData("11111111-1111-1111-1111-111111111111")]
    [InlineData(null)]
    public async Task GetOrderWithDetailsAsync__ShouldThrowUnauthorizedAccessException_WhenNotAuthorized(
        string? currentUserId)
    {
        // Arrange
        var orderId = Guid.NewGuid();
        var order = _fixture.Build<OrderDto>()
                                .With(o => o.Id, orderId)
                                .Create();

        _claimServiceMock.Setup(x => x.GetCurrentUserId)
            .Returns(currentUserId == null ? null : Guid.Parse(currentUserId));

        _claimServiceMock.Setup(x => x.IsAdmin).Returns(false);

        _orderRepoMock.Setup(x => x.GetOrderDetailAsync(orderId))
            .ReturnsAsync(order);

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() =>
            _orderService.GetOrderWithDetailsAsync(orderId));
    }

    [Fact]
    public async Task CreateOrderAsync_ShouldCreateOrder_WhenValid()
    {
        // Arrange
        var orderModel = _fixture.Build<OrderCreationModel>() 
                                 .With(o => o.PaymentMethod, PaymentMethodEnum.COD.ToString())
                                 .Create();

        var mockVariant = _fixture.Build<Variant>().Create();
        var customerId = Guid.NewGuid();

        _claimServiceMock.Setup(x => x.GetCurrentUserId).Returns(customerId);

        _variantRepoMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(mockVariant);

        // Act
        var result = await _orderService.CreateOrderAsync(orderModel);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(customerId, result.CustomerId);
        _variantRepoMock.Verify(x => x.GetByIdAsync(It.IsAny<Guid>()), 
                                Times.Exactly(orderModel.OrderItems.Count()));
        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task CreateOrderAsync_ShouldThrowArgumentException_WhenPaymentMethodInvalid()
    {
        // Arrange
        var orderModel = _fixture.Build<OrderCreationModel>()
                                 .With(o => o.PaymentMethod, "MOMO")
                                 .Create();

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ArgumentException>(() =>
            _orderService.CreateOrderAsync(orderModel));
        exception.Message.Should().Contain("Invalid payment method.");
    }

    [Fact]
    public async Task CreateOrderAsync_ShouldThrowArgumentException_WhenCustomerIdInvalid()
    {
        // Arrange
        var orderModel = _fixture.Build<OrderCreationModel>()
                                 .With(o => o.PaymentMethod, PaymentMethodEnum.COD.ToString())   
                                 .Create();
        _claimServiceMock.Setup(x => x.GetCurrentUserId).Returns(() => null);

        // Act & Assert
        var exception =  await Assert.ThrowsAsync<ArgumentException>(() =>
            _orderService.CreateOrderAsync(orderModel));
        exception.Message.Should().Be("You must login to make order.");
    }

    [Fact]
    public async Task CreateOrderAsync_ShouldThrowArgumentException_WhenVariantIdNotFound()
    {
        // Arrange
        var orderModel = _fixture.Build<OrderCreationModel>()
                                 .With(o => o.PaymentMethod, PaymentMethodEnum.COD.ToString())
                                 .Create();
        var variantId = orderModel.OrderItems.First().VariantId;

        var customerId = Guid.NewGuid();

        _claimServiceMock.Setup(x => x.GetCurrentUserId).Returns(customerId);

        _variantRepoMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(() => null);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ArgumentException>(() =>
            _orderService.CreateOrderAsync(orderModel));
        exception.Message.Should().Be($"Variant product with id {variantId} not found.");
    }
}
