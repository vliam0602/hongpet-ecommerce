using AutoFixture;
using FluentAssertions;
using HongPet.Application.Services.Abstractions;
using HongPet.Domain.Entities;
using HongPet.Domain.Test;
using HongPet.SharedViewModels.Generals;
using HongPet.SharedViewModels.Models;
using HongPet.SharedViewModels.ViewModels;
using HongPet.WebApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace HongPet.WebApi.Test.Controllers;

public class OrdersControllerTest : SetupTest
{
    private readonly OrdersController _ordersController;
    private readonly Mock<IOrderService> _orderServiceMock;

    public OrdersControllerTest()
    {
        // Mock IOrderService
        _orderServiceMock = _fixture.Freeze<Mock<IOrderService>>();

        // Initialize OrdersController
        _ordersController = new OrdersController(
            _fixture.Freeze<Mock<ILogger<OrdersController>>>().Object,
            _orderServiceMock.Object,
            _mapper);
    }

    [Fact]
    public async Task CreateOrder_ShouldReturnCreated_WhenOrderIsCreated()
    {
        // Arrange
        var orderModel = _fixture.Create<OrderCreationModel>();
        var createdOrder = _fixture.Create<Order>();
        _orderServiceMock.Setup(s => s.CreateOrderAsync(orderModel)).ReturnsAsync(createdOrder);

        // Act
        var result = await _ordersController.CreateOrder(orderModel);

        // Assert
        result.Should().BeOfType<CreatedAtActionResult>();
        var createdResult = result as CreatedAtActionResult;
        createdResult.Should().NotBeNull();
        createdResult!.StatusCode.Should().Be(201);
        var response = createdResult.Value as ApiResponse;
        response.Should().NotBeNull();
        response!.IsSuccess.Should().BeTrue();
        response.Message.Should().Contain("Order created successfully");
        response.Data.Should().Be(createdOrder.Id);
    }

    [Fact]
    public async Task CreateOrder_ShouldReturnBadRequest_WhenArgumentExceptionIsThrown()
    {
        // Arrange
        var orderModel = _fixture.Create<OrderCreationModel>();
        _orderServiceMock.Setup(s => s.CreateOrderAsync(orderModel))
                         .ThrowsAsync(new ArgumentException("Invalid order data"));

        // Act
        var result = await _ordersController.CreateOrder(orderModel);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
        var badRequestResult = result as BadRequestObjectResult;
        badRequestResult.Should().NotBeNull();
        badRequestResult!.StatusCode.Should().Be(400);
        var response = badRequestResult.Value as ApiResponse;
        response.Should().NotBeNull();
        response!.IsSuccess.Should().BeFalse();
        response.ErrorMessage.Should().Contain("Invalid order data");
    }

    [Fact]
    public async Task CreateOrder_ShouldReturnInternalServerError_WhenExceptionIsThrown()
    {
        // Arrange
        var orderModel = _fixture.Create<OrderCreationModel>();
        _orderServiceMock.Setup(s => s.CreateOrderAsync(orderModel))
                         .ThrowsAsync(new Exception("Unexpected error"));

        // Act
        var result = await _ordersController.CreateOrder(orderModel);

        // Assert
        result.Should().BeOfType<ObjectResult>();
        var objectResult = result as ObjectResult;
        objectResult.Should().NotBeNull();
        objectResult!.StatusCode.Should().Be(500);
        var response = objectResult.Value as ApiResponse;
        response.Should().NotBeNull();
        response!.IsSuccess.Should().BeFalse();
        response.ErrorMessage.Should().Contain("Unexpected error");
    }

    [Fact]
    public async Task GetUserOrders_ShouldReturnOk_WhenOrdersExist()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var criteria = _fixture.Create<QueryListCriteria>();
        var orders = _fixture.Create<PagedList<OrderVM>>();
        _orderServiceMock.Setup(s => s.GetOrdersByCustomerIdAsync(userId, criteria.PageIndex, criteria.PageSize, criteria.Keyword))
                         .ReturnsAsync(orders);

        // Act
        var result = await _ordersController.GetUserOrders(userId, criteria);

        // Assert
        result.Result.Should().BeOfType<OkObjectResult>();
        var okResult = result.Result as OkObjectResult;
        okResult.Should().NotBeNull();
        okResult!.StatusCode.Should().Be(200);
        var response = okResult.Value as ApiResponse;
        response.Should().NotBeNull();
        response!.IsSuccess.Should().BeTrue();
        response.Data.Should().Be(orders);
    }

    [Fact]
    public async Task GetUserOrders_ShouldReturnUnauthorized_WhenUnauthorizedAccessExceptionIsThrown()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var criteria = _fixture.Create<QueryListCriteria>();
        _orderServiceMock.Setup(s => s.GetOrdersByCustomerIdAsync(userId, criteria.PageIndex, criteria.PageSize, criteria.Keyword))
                         .ThrowsAsync(new UnauthorizedAccessException("Unauthorized access"));

        // Act
        var result = await _ordersController.GetUserOrders(userId, criteria);

        // Assert
        result.Result.Should().BeOfType<UnauthorizedObjectResult>();
        var unauthorizedResult = result.Result as UnauthorizedObjectResult;
        unauthorizedResult.Should().NotBeNull();
        unauthorizedResult!.StatusCode.Should().Be(401);
        var response = unauthorizedResult.Value as ApiResponse;
        response.Should().NotBeNull();
        response!.IsSuccess.Should().BeFalse();
        response.ErrorMessage.Should().Contain("Unauthorized access");
    }

    [Fact]
    public async Task GetUserOrders_ShouldReturnInternalServerError_WhenExceptionIsThrown()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var criteria = _fixture.Create<QueryListCriteria>();
        _orderServiceMock.Setup(s => s.GetOrdersByCustomerIdAsync(userId, criteria.PageIndex, criteria.PageSize, criteria.Keyword))
                         .ThrowsAsync(new Exception("Unexpected error"));

        // Act
        var result = await _ordersController.GetUserOrders(userId, criteria);

        // Assert
        result.Result.Should().BeOfType<ObjectResult>();
        var objectResult = result.Result as ObjectResult;
        objectResult.Should().NotBeNull();
        objectResult!.StatusCode.Should().Be(500);
        var response = objectResult.Value as ApiResponse;
        response.Should().NotBeNull();
        response!.IsSuccess.Should().BeFalse();
        response.ErrorMessage.Should().Contain("Unexpected error");
    }

    [Fact]
    public async Task GetAllOrders_ShouldReturnOk_WhenOrdersExist()
    {
        // Arrange
        var criteria = _fixture.Create<QueryListCriteria>();
        var orders = _fixture.Create<PagedList<Order>>();
        _orderServiceMock.Setup(s => s.GetPagedAsync(criteria.PageIndex, criteria.PageSize, criteria.Keyword))
                         .ReturnsAsync(orders);

        // Act
        var result = await _ordersController.GetAllOrders(criteria);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = result as OkObjectResult;
        okResult.Should().NotBeNull();
        okResult!.StatusCode.Should().Be(200);
        var response = okResult.Value as ApiResponse;
        response.Should().NotBeNull();
        response!.IsSuccess.Should().BeTrue();
        response.Data.Should().BeOfType<PagedList<OrderVM>>();
    }

    [Fact]
    public async Task GetAllOrders_ShouldReturnInternalServerError_WhenExceptionIsThrown()
    {
        // Arrange
        var criteria = _fixture.Create<QueryListCriteria>();
        _orderServiceMock.Setup(s => s.GetPagedAsync(criteria.PageIndex, criteria.PageSize, criteria.Keyword))
                         .ThrowsAsync(new Exception("Unexpected error"));

        // Act
        var result = await _ordersController.GetAllOrders(criteria);

        // Assert
        result.Should().BeOfType<ObjectResult>();
        var objectResult = result as ObjectResult;
        objectResult.Should().NotBeNull();
        objectResult!.StatusCode.Should().Be(500);
        var response = objectResult.Value as ApiResponse;
        response.Should().NotBeNull();
        response!.IsSuccess.Should().BeFalse();
        response.ErrorMessage.Should().Contain("Unexpected error");
    }

    [Fact]
    public async Task GetOrderDetail_ShouldReturnOk_WhenOrderExists()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        var order = _fixture.Create<OrderVM>();
        _orderServiceMock.Setup(s => s.GetOrderWithDetailsAsync(orderId)).ReturnsAsync(order);

        // Act
        var result = await _ordersController.GetOrderDetail(orderId);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = result as OkObjectResult;
        okResult.Should().NotBeNull();
        okResult!.StatusCode.Should().Be(200);
        var response = okResult.Value as ApiResponse;
        response.Should().NotBeNull();
        response!.IsSuccess.Should().BeTrue();
        response.Data.Should().Be(order);
    }

    [Fact]
    public async Task GetOrderDetail_ShouldReturnNotFound_WhenOrderDoesNotExist()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        _orderServiceMock.Setup(s => s.GetOrderWithDetailsAsync(orderId))
                         .ThrowsAsync(new KeyNotFoundException("Order not found"));

        // Act
        var result = await _ordersController.GetOrderDetail(orderId);

        // Assert
        result.Should().BeOfType<NotFoundObjectResult>();
        var notFoundResult = result as NotFoundObjectResult;
        notFoundResult.Should().NotBeNull();
        notFoundResult!.StatusCode.Should().Be(404);
        var response = notFoundResult.Value as ApiResponse;
        response.Should().NotBeNull();
        response!.IsSuccess.Should().BeFalse();
        response.ErrorMessage.Should().Contain("Order not found");
    }

    [Fact]
    public async Task GetOrderDetail_ShouldReturnUnauthorized_WhenUnauthorizedAccessExceptionIsThrown()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        _orderServiceMock.Setup(s => s.GetOrderWithDetailsAsync(orderId))
                         .ThrowsAsync(new UnauthorizedAccessException("Unauthorized access"));

        // Act
        var result = await _ordersController.GetOrderDetail(orderId);

        // Assert
        result.Should().BeOfType<UnauthorizedObjectResult>();
        var unauthorizedResult = result as UnauthorizedObjectResult;
        unauthorizedResult.Should().NotBeNull();
        unauthorizedResult!.StatusCode.Should().Be(401);
        var response = unauthorizedResult.Value as ApiResponse;
        response.Should().NotBeNull();
        response!.IsSuccess.Should().BeFalse();
        response.ErrorMessage.Should().Contain("Unauthorized access");
    }

    [Fact]
    public async Task GetOrderDetail_ShouldReturnInternalServerError_WhenExceptionIsThrown()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        _orderServiceMock.Setup(s => s.GetOrderWithDetailsAsync(orderId))
                         .ThrowsAsync(new Exception("Unexpected error"));

        // Act
        var result = await _ordersController.GetOrderDetail(orderId);

        // Assert
        result.Should().BeOfType<ObjectResult>();
        var objectResult = result as ObjectResult;
        objectResult.Should().NotBeNull();
        objectResult!.StatusCode.Should().Be(500);
        var response = objectResult.Value as ApiResponse;
        response.Should().NotBeNull();
        response!.IsSuccess.Should().BeFalse();
        response.ErrorMessage.Should().Contain("Unexpected error");
    }
}
