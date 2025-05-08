using AutoFixture;
using HongPet.Application.Services;
using HongPet.Domain.Entities;
using HongPet.Domain.Repositories.Abstractions;
using HongPet.Domain.Test;
using HongPet.SharedViewModels.Generals;
using HongPet.SharedViewModels.Models;
using Moq;

namespace HongPet.Application.Test.Services;

public class CategoryServiceTest : SetupTest
{
    private readonly CategoryService _categoryService;
    private readonly Mock<ICategoryRepository> _categoryRepoMock;

    public CategoryServiceTest()
    {
        // Mock the CategoryRepository & setup in the UnitOfWork
        _categoryRepoMock = new Mock<ICategoryRepository>();
        _unitOfWorkMock.SetupGet(x => x.CategoryRepository)
                       .Returns(_categoryRepoMock.Object);
        // Initialize the service with mocked dependencies
        _categoryService = new CategoryService(
            _unitOfWorkMock.Object, 
            _claimServiceMock.Object, 
            _mapper);        
    }

    [Fact]
    public async Task GetPagedCategoriesAsync_ShouldReturnPagedCategories()
    {
        // Arrange
        var mockCategories = _fixture.Build<Category>()
                                     .CreateMany(10)
                                     .ToList();    

        var pagedCategories = new PagedList<Category>(mockCategories, 10, 1, 10);        

        _categoryRepoMock.Setup(x => x.GetPagedAsync(1, 10, ""))
                         .ReturnsAsync(pagedCategories);

        // Act
        var result = await _categoryService.GetPagedCategoriesAsync(1, 10, "");

        // Assert
        Assert.NotNull(result);
        Assert.Equal(10, result.Items.Count);
        _unitOfWorkMock.Verify(u => 
            u.CategoryRepository.GetPagedAsync(1, 10, ""), Times.Once);
    }

    [Fact]
    public async Task UpdateCategoryAsync_ShouldUpdateCategory_WhenCategoryExists()
    {
        // Arrange
        var categoryId = Guid.NewGuid();
        var existingCategory = _fixture.Build<Category>()
                                       .With(c => c.Id, categoryId)
                                       .Without(c => c.SubCategories)                                       
                                       .Without(c => c.Products)
                                       .Without(c => c.DeletedDate)
                                       .Without(c => c.CreatedBy)
                                       .Create();

        var updateModel = new CategoryCreateModel
        {
            Name = "Updated Category",
            ParentCategoryId = null
        };

        _categoryRepoMock.Setup(x => x.GetByIdAsync(categoryId))
                       .ReturnsAsync(existingCategory);

        // Act
        var result = await _categoryService.UpdateCategoryAsync(categoryId, updateModel);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Updated Category", result.Name);
        _unitOfWorkMock.Verify(u => u.CategoryRepository.GetByIdAsync(categoryId), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task UpdateCategoryAsync_ShouldThrowException_WhenCategoryNotFound()
    {
        // Arrange
        var categoryId = Guid.NewGuid();
        var updateModel = new CategoryCreateModel
        {
            Name = "Updated Category",
            ParentCategoryId = null
        };

        _categoryRepoMock.Setup(x => x.GetByIdAsync(categoryId))
                       .ReturnsAsync((Category?)null);

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() =>
            _categoryService.UpdateCategoryAsync(categoryId, updateModel));

        _unitOfWorkMock.Verify(u => u.CategoryRepository.GetByIdAsync(categoryId), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Never);
    }
}
