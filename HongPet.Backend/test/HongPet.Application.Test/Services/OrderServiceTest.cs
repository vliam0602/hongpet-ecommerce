using AutoFixture;
using AutoMapper;
using HongPet.Application.Services;
using HongPet.Application.Services.Abstractions;
using HongPet.Application.Services.Commons;
using HongPet.Domain.Entities;
using HongPet.Domain.Enums;
using HongPet.Domain.Repositories.Abstractions;
using HongPet.Domain.Test;
using HongPet.Infrastructure.DTOs;
using HongPet.SharedViewModels.Generals;
using HongPet.SharedViewModels.Models;
using HongPet.SharedViewModels.ViewModels;
using Moq;

namespace HongPet.Application.Test.Services
{
    public class OrderServiceTest : SetupTest
    {
        private readonly Mock<IOrderRepository> _orderRepositoryMock;
        private readonly Mock<IGenericRepository<Variant>> _variantRepositoryMock;
        private readonly Mock<IClaimService> _claimServiceMock;
        private readonly OrderService _orderService;

        public OrderServiceTest()
        {
            _orderRepositoryMock = new Mock<IOrderRepository>();
            _variantRepositoryMock = new Mock<IGenericRepository<Variant>>();
            _claimServiceMock = new Mock<IClaimService>();

            _unitOfWorkMock
                .Setup(u => u.OrderRepository)
                .Returns(_orderRepositoryMock.Object);

            _unitOfWorkMock
                .Setup(u => u.Repository<Variant>())
                .Returns(_variantRepositoryMock.Object);

            _orderService = new OrderService(
                _unitOfWorkMock.Object, _mapper, _claimServiceMock.Object);
        }

        [Fact]
        public async Task CreateOrderAsync_ShouldCreateOrder_WhenValidInput()
        {
            // Arrange
            var customerId = Guid.NewGuid();
            var orderModel = new OrderCreationModel
            {
                CustomerName = "John Doe",
                CustomerPhone = "123456789",
                CustomerEmail = "john.doe@example.com",
                ShippingAddress = "123 Main St",
                PaymentMethod = PaymentMethodEnum.COD.ToString(),
                OrderItems = new List<OrderItemCreationModel>
                {
                    new OrderItemCreationModel
                    {
                        VariantId = Guid.NewGuid(),
                        Quantity = 2,
                        Price = 100
                    }
                }
            };

            var variant = new Variant 
            { Id = orderModel.OrderItems.First().VariantId, Stock = 10 };

            _claimServiceMock.Setup(c => c.GetCurrentUserId).Returns(customerId);
            _variantRepositoryMock.Setup(v => v.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(variant);

            // Act
            var result = await _orderService.CreateOrderAsync(orderModel);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(customerId, result.CustomerId);
            Assert.Equal(200, result.TotalAmount);
            _variantRepositoryMock.Verify(v => v.Update(It.IsAny<Variant>()), Times.Once);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
        }
        

        [Fact]
        public async Task GetOrderWithDetailsAsync_ShouldReturnOrder_WhenAuthorized()
        {
            // Arrange
            var orderId = Guid.NewGuid();
            var customerId = Guid.NewGuid();
            var order = OrderDtosMockData(1).First();
            order.CustomerId = customerId;

            _claimServiceMock.Setup(c => c.GetCurrentUserId).Returns(customerId);
            _claimServiceMock.Setup(c => c.IsAdmin).Returns(false);
            _orderRepositoryMock.Setup(o => o.GetOrderDetailAsync(orderId)).ReturnsAsync(order);

            // Act
            var result = await _orderService.GetOrderWithDetailsAsync(orderId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(order.CustomerId, result.CustomerId);
        }

        [Fact]
        public async Task CreateOrderAsync_ShouldThrowException_WhenVariantNotFound()
        {
            // Arrange
            var customerId = Guid.NewGuid();
            var orderModel = new OrderCreationModel
            {
                CustomerName = "John Doe",
                CustomerPhone = "123456789",
                CustomerEmail = "john.doe@example.com",
                ShippingAddress = "123 Main St",
                PaymentMethod = PaymentMethodEnum.COD.ToString(),
                OrderItems = new List<OrderItemCreationModel>
                {
                    new OrderItemCreationModel
                    {
                        VariantId = Guid.NewGuid(),
                        Quantity = 2,
                        Price = 100
                    }
                }
            };

            _claimServiceMock.Setup(c => c.GetCurrentUserId).Returns(customerId);
            _variantRepositoryMock.Setup(v => v.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Variant?)null);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _orderService.CreateOrderAsync(orderModel));
        }
    }
}
