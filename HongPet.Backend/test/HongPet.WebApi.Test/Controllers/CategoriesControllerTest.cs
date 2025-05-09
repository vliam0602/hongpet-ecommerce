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

public class CategoriesControllerTest : SetupTest
{
    private readonly CategoriesController _categoriesController;
    private readonly Mock<ICategoryService> _categoryServiceMock;

    public CategoriesControllerTest()
    {
        // Mock ICategoryService
        _categoryServiceMock = _fixture.Freeze<Mock<ICategoryService>>();

        // Initialize CategoriesController
        _categoriesController = new CategoriesController(
            _fixture.Freeze<Mock<ILogger<CategoriesController>>>().Object,
            _categoryServiceMock.Object,
            _mapper);
    }

    [Fact]
    public async Task GetCategories_ShouldReturnOk_WhenDataExists()
    {
        // Arrange
        var pagedCategories = _fixture.Create<PagedList<CategoryVM>>();
        var criteria = _fixture.Create<QueryListCriteria>();
        _categoryServiceMock.Setup(s => s.GetPagedCategoriesAsync(
                                    criteria.PageIndex, criteria.PageSize, criteria.Keyword))
                            .ReturnsAsync(pagedCategories);

        // Act
        var result = await _categoriesController.GetCategories(criteria);

        // Assert
        result.Result.Should().BeOfType<OkObjectResult>();
        var okResult = result.Result as OkObjectResult;
        okResult.Should().NotBeNull();
        okResult!.StatusCode.Should().Be(200); // Status code 200
        var response = okResult.Value as ApiResponse;
        response.Should().NotBeNull();
        response!.IsSuccess.Should().BeTrue();
        response.Data.Should().Be(pagedCategories);
    }

    [Fact]
    public async Task GetCategories_ShouldReturnInternalServerError_WhenExceptionIsThrown()
    {
        // Arrange
        var criteria = _fixture.Create<QueryListCriteria>();
        _categoryServiceMock.Setup(s => s.GetPagedCategoriesAsync(
                                    criteria.PageIndex, criteria.PageSize, criteria.Keyword))
                            .ThrowsAsync(new Exception("Unexpected error"));

        // Act
        var result = await _categoriesController.GetCategories(criteria);

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
    public async Task GetAllCategories_ShouldReturnOk_WhenDataExists()
    {
        // Arrange
        var categories = _fixture.Create<List<Category>>();
        _categoryServiceMock.Setup(s => s.GetAllAsync()).ReturnsAsync(categories);

        // Act
        var result = await _categoriesController.GetAllCategories();

        // Assert
        result.Result.Should().BeOfType<OkObjectResult>();
        var okResult = result.Result as OkObjectResult;
        okResult.Should().NotBeNull();
        okResult!.StatusCode.Should().Be(200); // Status code 200
        var response = okResult.Value as ApiResponse;
        response.Should().NotBeNull();
        response!.IsSuccess.Should().BeTrue();
        response.Data.Should().Be(categories);
    }

    [Fact]
    public async Task GetAllCategories_ShouldReturnInternalServerError_WhenExceptionIsThrown()
    {
        // Arrange
        _categoryServiceMock.Setup(s => s.GetAllAsync()).ThrowsAsync(new Exception("Unexpected error"));

        // Act
        var result = await _categoriesController.GetAllCategories();

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
    public async Task CreateCategory_ShouldReturnCreated_WhenCategoryIsCreated()
    {
        // Arrange
        var categoryModel = _fixture.Create<CategoryCreateModel>();
        var createdId = Guid.NewGuid();
        _categoryServiceMock.Setup(s => s.AddAsync(It.IsAny<Category>())).ReturnsAsync(createdId);

        // Act
        var result = await _categoriesController.CreateCategory(categoryModel);

        // Assert
        result.Should().BeOfType<CreatedAtActionResult>();
        var createdResult = result as CreatedAtActionResult;
        createdResult.Should().NotBeNull();
        createdResult!.StatusCode.Should().Be(201); // Status code 201
        var response = createdResult.Value as ApiResponse;
        response.Should().NotBeNull();
        response!.IsSuccess.Should().BeTrue();
        response.Message.Should().Contain(categoryModel.Name);
        response.Data.Should().Be(createdId);
    }

    [Fact]
    public async Task CreateCategory_ShouldReturnInternalServerError_WhenExceptionIsThrown()
    {
        // Arrange
        var categoryModel = _fixture.Create<CategoryCreateModel>();
        _categoryServiceMock.Setup(s => s.AddAsync(It.IsAny<Category>())).ThrowsAsync(new Exception("Unexpected error"));

        // Act
        var result = await _categoriesController.CreateCategory(categoryModel);

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
    public async Task GetCategoryDetail_ShouldReturnOk_WhenCategoryExists()
    {
        // Arrange
        var category = _fixture.Create<Category>();
        var categoryId = category.Id;
        _categoryServiceMock.Setup(s => s.GetByIdAsync(categoryId)).ReturnsAsync(category);

        // Act
        var result = await _categoriesController.GetCategoryDetail(categoryId);

        // Assert
        result.Result.Should().BeOfType<OkObjectResult>();
        var okResult = result.Result as OkObjectResult;
        okResult.Should().NotBeNull();
        okResult!.StatusCode.Should().Be(200); // Status code 200
        var response = okResult.Value as ApiResponse;
        response.Should().NotBeNull();
        response!.IsSuccess.Should().BeTrue();
        response.Data.Should().BeOfType<CategoryVM>();
    }

    [Fact]
    public async Task GetCategoryDetail_ShouldReturnNotFound_WhenCategoryDoesNotExist()
    {
        // Arrange
        var categoryId = Guid.NewGuid();
        _categoryServiceMock.Setup(s => s.GetByIdAsync(categoryId)).ReturnsAsync((Category?)null);

        // Act
        var result = await _categoriesController.GetCategoryDetail(categoryId);

        // Assert
        result.Result.Should().BeOfType<NotFoundObjectResult>();
        var notFoundResult = result.Result as NotFoundObjectResult;
        notFoundResult.Should().NotBeNull();
        notFoundResult!.StatusCode.Should().Be(404); // Status code 404
        var response = notFoundResult.Value as ApiResponse;
        response.Should().NotBeNull();
        response!.IsSuccess.Should().BeFalse();
        response.ErrorMessage.Should().Contain(categoryId.ToString());
    }

    [Fact]
    public async Task GetCategoryDetail_ShouldReturnInternalServerError_WhenExceptionIsThrown()
    {
        // Arrange
        var categoryId = Guid.NewGuid();
        _categoryServiceMock.Setup(s => s.GetByIdAsync(categoryId))
                            .ThrowsAsync(new Exception("Unexpected error"));

        // Act
        var result = await _categoriesController.GetCategoryDetail(categoryId);

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
    public async Task UpdateCategory_ShouldReturnOk_WhenCategoryIsUpdated()
    {
        // Arrange
        var categoryId = Guid.NewGuid();
        var categoryModel = _fixture.Create<CategoryCreateModel>();
        var updatedCategory = _fixture.Create<CategoryVM>();
        _categoryServiceMock.Setup(s => s.UpdateCategoryAsync(categoryId, categoryModel))
                            .ReturnsAsync(updatedCategory);

        // Act
        var result = await _categoriesController.UpdateCategory(categoryId, categoryModel);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = result as OkObjectResult;
        okResult.Should().NotBeNull();
        okResult!.StatusCode.Should().Be(200); // Status code 200
        var response = okResult.Value as ApiResponse;
        response.Should().NotBeNull();
        response!.IsSuccess.Should().BeTrue();
        response.Message.Should().Contain(categoryId.ToString());
        response.Data.Should().Be(updatedCategory);
    }

    [Fact]
    public async Task UpdateCategory_ShouldReturnNotFound_WhenCategoryDoesNotExist()
    {
        // Arrange
        var categoryId = Guid.NewGuid();
        var categoryModel = _fixture.Create<CategoryCreateModel>();
        _categoryServiceMock.Setup(s => s.UpdateCategoryAsync(categoryId, categoryModel))
                            .ThrowsAsync(new KeyNotFoundException("Category not found"));

        // Act
        var result = await _categoriesController.UpdateCategory(categoryId, categoryModel);

        // Assert
        result.Should().BeOfType<NotFoundObjectResult>();
        var notFoundResult = result as NotFoundObjectResult;
        notFoundResult.Should().NotBeNull();
        notFoundResult!.StatusCode.Should().Be(404); // Status code 404
        var response = notFoundResult.Value as ApiResponse;
        response.Should().NotBeNull();
        response!.IsSuccess.Should().BeFalse();
        response.ErrorMessage.Should().Contain("Category not found");
    }

    [Fact]
    public async Task UpdateCategory_ShouldReturnInternalServerError_WhenExceptionIsThrown()
    {
        // Arrange
        var categoryId = Guid.NewGuid();
        var categoryModel = _fixture.Create<CategoryCreateModel>();
        _categoryServiceMock.Setup(s => s.UpdateCategoryAsync(categoryId, categoryModel))
                            .ThrowsAsync(new Exception("Unexpected error"));

        // Act
        var result = await _categoriesController.UpdateCategory(categoryId, categoryModel);

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
    public async Task DeleteCategory_ShouldReturnOk_WhenCategoryIsDeleted()
    {
        // Arrange
        var categoryId = Guid.NewGuid();
        _categoryServiceMock.Setup(s => s.DeleteAsync(categoryId)).Returns(Task.CompletedTask);

        // Act
        var result = await _categoriesController.DeleteCategory(categoryId);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = result as OkObjectResult;
        okResult.Should().NotBeNull();
        okResult!.StatusCode.Should().Be(200); // Status code 200
        var response = okResult.Value as ApiResponse;
        response.Should().NotBeNull();
        response!.IsSuccess.Should().BeTrue();
        response.Message.Should().Contain(categoryId.ToString());
    }

    [Fact]
    public async Task DeleteCategory_ShouldReturnNotFound_WhenCategoryDoesNotExist()
    {
        // Arrange
        var categoryId = Guid.NewGuid();
        _categoryServiceMock.Setup(s => s.DeleteAsync(categoryId))
                            .ThrowsAsync(new KeyNotFoundException("Category not found"));

        // Act
        var result = await _categoriesController.DeleteCategory(categoryId);

        // Assert
        result.Should().BeOfType<NotFoundObjectResult>();
        var notFoundResult = result as NotFoundObjectResult;
        notFoundResult.Should().NotBeNull();
        notFoundResult!.StatusCode.Should().Be(404); // Status code 404
        var response = notFoundResult.Value as ApiResponse;
        response.Should().NotBeNull();
        response!.IsSuccess.Should().BeFalse();
        response.ErrorMessage.Should().Contain("Category not found");
    }

    [Fact]
    public async Task DeleteCategory_ShouldReturnInternalServerError_WhenExceptionIsThrown()
    {
        // Arrange
        var categoryId = Guid.NewGuid();
        _categoryServiceMock.Setup(s => s.DeleteAsync(categoryId))
                            .ThrowsAsync(new Exception("Unexpected error"));

        // Act
        var result = await _categoriesController.DeleteCategory(categoryId);

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
}
