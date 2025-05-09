using AutoFixture;
using FluentAssertions;
using HongPet.Application.Commons;
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

public class ProductsControllerTest : SetupTest
{
    private readonly ProductsController _productsController;
    private readonly Mock<IProductService> _productServiceMock;
    private readonly Mock<IReviewService> _reviewServiceMock;

    public ProductsControllerTest()
    {
        // Mock services
        _productServiceMock = _fixture.Freeze<Mock<IProductService>>();
        _reviewServiceMock = _fixture.Freeze<Mock<IReviewService>>();

        // Initialize ProductsController
        _productsController = new ProductsController(
            _fixture.Freeze<Mock<ILogger<ProductsController>>>().Object,
            _productServiceMock.Object,
            _reviewServiceMock.Object);
    }

    [Fact]
    public async Task GetProducts_ShouldReturnOk_WhenDataExists()
    {
        // Arrange
        var pagedProducts = _fixture.Create<PagedList<ProductGeneralVM>>();
        var criteria = _fixture.Create<QueryListCriteria>();
        _productServiceMock.Setup(s => s.GetPagedProductAsync(
                                    criteria.PageIndex, criteria.PageSize, criteria.Keyword, null))
                           .ReturnsAsync(pagedProducts);

        // Act
        var result = await _productsController.GetProducts(criteria);

        // Assert
        result.Result.Should().BeOfType<OkObjectResult>();
        var okResult = result.Result as OkObjectResult;
        okResult.Should().NotBeNull();
        okResult!.StatusCode.Should().Be(200); // Status code 200
        var response = okResult.Value as ApiResponse;
        response.Should().NotBeNull();
        response!.IsSuccess.Should().BeTrue();
        response.Data.Should().Be(pagedProducts);
    }

    [Fact]
    public async Task GetProducts_ShouldReturnInternalServerError_WhenExceptionIsThrown()
    {
        // Arrange
        var criteria = _fixture.Create<QueryListCriteria>();
        _productServiceMock.Setup(s => s.GetPagedProductAsync(
                                    criteria.PageIndex, criteria.PageSize, criteria.Keyword, null))
                           .ThrowsAsync(new Exception("Unexpected error"));

        // Act
        var result = await _productsController.GetProducts(criteria);

        // Assert
        result.Result.Should().BeOfType<ObjectResult>();
        var objectResult = result.Result as ObjectResult;
        objectResult.Should().NotBeNull();
        objectResult!.StatusCode.Should().Be(500); // Status code 500
        var response = objectResult.Value as ApiResponse;
        response.Should().NotBeNull();
        response!.IsSuccess.Should().BeFalse();
        response.ErrorMessage.Should().Contain("Unexpected error");
    }

    [Fact]
    public async Task GetProduct_ShouldReturnOk_WhenProductExists()
    {
        // Arrange
        var productId = Guid.NewGuid();
        var productDetail = _fixture.Create<ProductDetailVM>();
        _productServiceMock.Setup(s => s.GetProductDetailAsync(productId)).ReturnsAsync(productDetail);

        // Act
        var result = await _productsController.GetProduct(productId);

        // Assert
        result.Result.Should().BeOfType<OkObjectResult>();
        var okResult = result.Result as OkObjectResult;
        okResult.Should().NotBeNull();
        okResult!.StatusCode.Should().Be(200); // Status code 200
        var response = okResult.Value as ApiResponse;
        response.Should().NotBeNull();
        response!.IsSuccess.Should().BeTrue();
        response.Data.Should().Be(productDetail);
    }

    [Fact]
    public async Task GetProduct_ShouldReturnNotFound_WhenProductDoesNotExist()
    {
        // Arrange
        var productId = Guid.NewGuid();
        _productServiceMock.Setup(s => s.GetProductDetailAsync(productId))
                           .ThrowsAsync(new KeyNotFoundException());

        // Act
        var result = await _productsController.GetProduct(productId);

        // Assert
        result.Result.Should().BeOfType<NotFoundObjectResult>();
        var notFoundResult = result.Result as NotFoundObjectResult;
        notFoundResult.Should().NotBeNull();
        notFoundResult!.StatusCode.Should().Be(404); // Status code 404
        var response = notFoundResult.Value as ApiResponse;
        response.Should().NotBeNull();
        response!.IsSuccess.Should().BeFalse();
        response.ErrorMessage.Should().Be("Product is not exist!");
    }

    [Fact]
    public async Task GetProduct_ShouldReturnInternalServerError_WhenExceptionIsThrown()
    {
        // Arrange
        var productId = Guid.NewGuid();
        _productServiceMock.Setup(s => s.GetProductDetailAsync(productId))
                           .ThrowsAsync(new Exception("Unexpected error"));

        // Act
        var result = await _productsController.GetProduct(productId);

        // Assert
        result.Result.Should().BeOfType<ObjectResult>();
        var objectResult = result.Result as ObjectResult;
        objectResult.Should().NotBeNull();
        objectResult!.StatusCode.Should().Be(500); // Status code 500
        var response = objectResult.Value as ApiResponse;
        response.Should().NotBeNull();
        response!.IsSuccess.Should().BeFalse();
        response.ErrorMessage.Should().Contain("Unexpected error");
    }

    [Fact]
    public async Task AddProduct_ShouldReturnCreated_WhenProductIsAdded()
    {
        // Arrange
        var productModel = _fixture.Create<ProductModel>();
        var createdProductId = Guid.NewGuid();
        _productServiceMock.Setup(s => s.AddProductAsync(productModel)).ReturnsAsync(createdProductId);

        // Act
        var result = await _productsController.AddProduct(productModel);

        // Assert
        result.Should().BeOfType<CreatedAtActionResult>();
        var createdResult = result as CreatedAtActionResult;
        createdResult.Should().NotBeNull();
        createdResult!.StatusCode.Should().Be(201); // Status code 201
        var response = createdResult.Value as ApiResponse;
        response.Should().NotBeNull();
        response!.IsSuccess.Should().BeTrue();
        response.Message.Should().Be("Product added successfully.");
        response.Data.Should().BeEquivalentTo(new { Id = createdProductId });
    }

    [Fact]
    public async Task AddProduct_ShouldReturnBadRequest_WhenKeyNotFoundExceptionIsThrown()
    {
        // Arrange
        var productModel = _fixture.Create<ProductModel>();
        var exceptionMessage = "Category not found";
        _productServiceMock.Setup(s => s.AddProductAsync(productModel))
                           .ThrowsAsync(new KeyNotFoundException(exceptionMessage));

        // Act
        var result = await _productsController.AddProduct(productModel);

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
    public async Task AddProduct_ShouldReturnInternalServerError_WhenExceptionIsThrown()
    {
        // Arrange
        var productModel = _fixture.Create<ProductModel>();
        _productServiceMock.Setup(s => s.AddProductAsync(productModel))
                           .ThrowsAsync(new Exception("Unexpected error"));

        // Act
        var result = await _productsController.AddProduct(productModel);

        // Assert
        result.Should().BeOfType<ObjectResult>();
        var objectResult = result as ObjectResult;
        objectResult.Should().NotBeNull();
        objectResult!.StatusCode.Should().Be(500); // Status code 500
        var response = objectResult.Value as ApiResponse;
        response.Should().NotBeNull();
        response!.IsSuccess.Should().BeFalse();
        response.ErrorMessage.Should().Contain("Unexpected error");
    }

    [Fact]
    public async Task UpdateProduct_ShouldReturnOk_WhenProductIsUpdated()
    {
        // Arrange
        var productId = Guid.NewGuid();
        var productModel = _fixture.Create<ProductModel>();
        var updatedProduct = _fixture.Create<ProductDetailVM>();
        _productServiceMock.Setup(s => s.UpdateProductAsync(productId, productModel))
                           .ReturnsAsync(updatedProduct);

        // Act
        var result = await _productsController.UpdateProduct(productId, productModel);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = result as OkObjectResult;
        okResult.Should().NotBeNull();
        okResult!.StatusCode.Should().Be(200); // Status code 200
        var response = okResult.Value as ApiResponse;
        response.Should().NotBeNull();
        response!.IsSuccess.Should().BeTrue();
        response.Message.Should().Contain(productId.ToString());
        response.Data.Should().Be(updatedProduct);
    }

    [Fact]
    public async Task UpdateProduct_ShouldReturnNotFound_WhenProductDoesNotExist()
    {
        // Arrange
        var productId = Guid.NewGuid();
        var productModel = _fixture.Create<ProductModel>();
        _productServiceMock.Setup(s => s.UpdateProductAsync(productId, productModel))
                           .ThrowsAsync(new KeyNotFoundException("Product not found"));

        // Act
        var result = await _productsController.UpdateProduct(productId, productModel);

        // Assert
        result.Should().BeOfType<NotFoundObjectResult>();
        var notFoundResult = result as NotFoundObjectResult;
        notFoundResult.Should().NotBeNull();
        notFoundResult!.StatusCode.Should().Be(404); // Status code 404
        var response = notFoundResult.Value as ApiResponse;
        response.Should().NotBeNull();
        response!.IsSuccess.Should().BeFalse();
        response.ErrorMessage.Should().Contain("Product not found");
    }

    [Fact]
    public async Task UpdateProduct_ShouldReturnInternalServerError_WhenExceptionIsThrown()
    {
        // Arrange
        var productId = Guid.NewGuid();
        var productModel = _fixture.Create<ProductModel>();
        _productServiceMock.Setup(s => s.UpdateProductAsync(productId, productModel))
                           .ThrowsAsync(new Exception("Unexpected error"));

        // Act
        var result = await _productsController.UpdateProduct(productId, productModel);

        // Assert
        result.Should().BeOfType<ObjectResult>();
        var objectResult = result as ObjectResult;
        objectResult.Should().NotBeNull();
        objectResult!.StatusCode.Should().Be(500); // Status code 500
        var response = objectResult.Value as ApiResponse;
        response.Should().NotBeNull();
        response!.IsSuccess.Should().BeFalse();
        response.ErrorMessage.Should().Contain("Unexpected error");
    }

    [Fact]
    public async Task SoftDeleteProduct_ShouldReturnOk_WhenProductIsDeleted()
    {
        // Arrange
        var productId = Guid.NewGuid();
        _productServiceMock.Setup(s => s.SoftDeleteProductAsync(productId)).Returns(Task.CompletedTask);

        // Act
        var result = await _productsController.SoftDeleteProduct(productId);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = result as OkObjectResult;
        okResult.Should().NotBeNull();
        okResult!.StatusCode.Should().Be(200); // Status code 200
        var response = okResult.Value as ApiResponse;
        response.Should().NotBeNull();
        response!.IsSuccess.Should().BeTrue();
        response.Message.Should().Contain(productId.ToString());
    }

    [Fact]
    public async Task SoftDeleteProduct_ShouldReturnNotFound_WhenProductDoesNotExist()
    {
        // Arrange
        var productId = Guid.NewGuid();
        _productServiceMock.Setup(s => s.SoftDeleteProductAsync(productId))
                           .ThrowsAsync(new KeyNotFoundException("Product not found"));

        // Act
        var result = await _productsController.SoftDeleteProduct(productId);

        // Assert
        result.Should().BeOfType<NotFoundObjectResult>();
        var notFoundResult = result as NotFoundObjectResult;
        notFoundResult.Should().NotBeNull();
        notFoundResult!.StatusCode.Should().Be(404); // Status code 404
        var response = notFoundResult.Value as ApiResponse;
        response.Should().NotBeNull();
        response!.IsSuccess.Should().BeFalse();
        response.ErrorMessage.Should().Contain("Product not found");
    }

    [Fact]
    public async Task SoftDeleteProduct_ShouldReturnInternalServerError_WhenExceptionIsThrown()
    {
        // Arrange
        var productId = Guid.NewGuid();
        _productServiceMock.Setup(s => s.SoftDeleteProductAsync(productId))
                           .ThrowsAsync(new Exception("Unexpected error"));

        // Act
        var result = await _productsController.SoftDeleteProduct(productId);

        // Assert
        result.Should().BeOfType<ObjectResult>();
        var objectResult = result as ObjectResult;
        objectResult.Should().NotBeNull();
        objectResult!.StatusCode.Should().Be(500); // Status code 500
        var response = objectResult.Value as ApiResponse;
        response.Should().NotBeNull();
        response!.IsSuccess.Should().BeFalse();
        response.ErrorMessage.Should().Contain("Unexpected error");
    }

    [Fact]
    public async Task GetAllAttributes_ShouldReturnOk_WhenAttributesExist()
    {
        // Arrange
        var attributes = _fixture.Create<List<AttributeVM>>();
        _productServiceMock.Setup(s => s.GetAllAttributes()).ReturnsAsync(attributes);

        // Act
        var result = await _productsController.GetAllAttributes();

        // Assert
        result.Result.Should().BeOfType<OkObjectResult>();
        var okResult = result.Result as OkObjectResult;
        okResult.Should().NotBeNull();
        okResult!.StatusCode.Should().Be(200); // Status code 200
        var response = okResult.Value as ApiResponse;
        response.Should().NotBeNull();
        response!.IsSuccess.Should().BeTrue();
        response.Data.Should().Be(attributes);
    }

    [Fact]
    public async Task GetAllAttributes_ShouldReturnInternalServerError_WhenExceptionIsThrown()
    {
        // Arrange
        _productServiceMock.Setup(s => s.GetAllAttributes())
                           .ThrowsAsync(new Exception("Unexpected error"));

        // Act
        var result = await _productsController.GetAllAttributes();

        // Assert
        result.Result.Should().BeOfType<ObjectResult>();
        var objectResult = result.Result as ObjectResult;
        objectResult.Should().NotBeNull();
        objectResult!.StatusCode.Should().Be(500); // Status code 500
        var response = objectResult.Value as ApiResponse;
        response.Should().NotBeNull();
        response!.IsSuccess.Should().BeFalse();
        response.ErrorMessage.Should().Contain("Unexpected error");
    }

    [Fact]
    public async Task GetProductReviews_ShouldReturnOk_WhenReviewsExist()
    {
        // Arrange
        var productId = Guid.NewGuid();
        var pagedReviews = _fixture.Create<PagedList<ReviewVM>>();
        var criteria = _fixture.Create<QueryListCriteria>();
        _reviewServiceMock.Setup(s => s.GetReviewsByProductIdAsync(
                                        productId, criteria.PageIndex, criteria.PageSize))
                          .ReturnsAsync(pagedReviews);

        // Act
        var result = await _productsController.GetProductReviews(productId, criteria);

        // Assert
        result.Result.Should().BeOfType<OkObjectResult>();
        var okResult = result.Result as OkObjectResult;
        okResult.Should().NotBeNull();
        okResult!.StatusCode.Should().Be(200); // Status code 200
        var response = okResult.Value as ApiResponse;
        response.Should().NotBeNull();
        response!.IsSuccess.Should().BeTrue();
        response.Data.Should().Be(pagedReviews);
    }

    [Fact]
    public async Task GetProductReviews_ShouldReturnInternalServerError_WhenExceptionIsThrown()
    {
        // Arrange
        var productId = Guid.NewGuid();
        var criteria = _fixture.Create<QueryListCriteria>();
        _reviewServiceMock.Setup(s => s.GetReviewsByProductIdAsync(
                                        productId, criteria.PageIndex, criteria.PageSize))
                          .ThrowsAsync(new Exception("Unexpected error"));

        // Act
        var result = await _productsController.GetProductReviews(productId, criteria);

        // Assert
        result.Result.Should().BeOfType<ObjectResult>();
        var objectResult = result.Result as ObjectResult;
        objectResult.Should().NotBeNull();
        objectResult!.StatusCode.Should().Be(500); // Status code 500
        var response = objectResult.Value as ApiResponse;
        response.Should().NotBeNull();
        response!.IsSuccess.Should().BeFalse();
        response.ErrorMessage.Should().Contain("Unexpected error");
    }

}
