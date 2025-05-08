using FluentAssertions;
using HongPet.Domain.Entities;
using HongPet.Domain.Test;
using HongPet.Infrastructure.Database;
using HongPet.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace HongPet.Infrastructure.Test.Repositories;

public class CategoryRepositoryTest : SetupTest
{
    private readonly CategoryRepository _categoryRepository;
    private readonly AppDbContext _context;

    public CategoryRepositoryTest()
    {
        _context = new AppDbContext(new DbContextOptionsBuilder<AppDbContext>()
                    .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                    .Options);

        // reset the database
        _context.Categories.RemoveRange(_context.Categories);
        _context.SaveChanges();

        // initialize the category repository
        _categoryRepository = new CategoryRepository(_context);        
    }


    [Theory]
    [InlineData(2, 2, 2)] // with the list count is 5
    [InlineData(3, 2, 1)]
    public async Task GetPagedAsync_ShouldReturnPagedCategories_WhenDataExists(
        int pageIndex, int pageSize, int expected)
    {
        // Arrange
        var categories = MockCategoriesNotDeleted(5);

        _context.Categories.AddRange(categories);
        await _context.SaveChangesAsync();

        // Act
        var result = await _categoryRepository.GetPagedAsync(pageIndex, pageSize);

        // Assert
        _context.Categories.Count().Should().Be(5);
        result.Should().NotBeNull();
        result.Items.Should().HaveCount(expected);
        result.TotalCount.Should().Be(categories.Count);
        result.CurrentPage.Should().Be(pageIndex);
        result.TotalPages.Should().Be((int)Math.Ceiling((double)categories.Count / pageSize));
    }
    

    [Fact]
    public async Task GetPagedAsync_ShouldFilterByKeyword_WhenKeywordIsProvided()
    {
        // Arrange
        var categories = MockCategoriesNotDeleted(5);

        categories[0].Name = "TestCategory";

        _context.Categories.AddRange(categories);
        await _context.SaveChangesAsync();

        // Act
        var result = await _categoryRepository.GetPagedAsync(
            pageIndex: 1, pageSize: 10, keyword: "Test");

        // Assert
        result.Should().NotBeNull();
        result.Items.Should().HaveCount(1);
        result.Items.First().Name.Should().Be("TestCategory");
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnCategoryWithParent_WhenCategoryExists()
    {
        // Arrange
        var parentCategory = MockCategoriesNotDeleted(1).First();
        var childCategory = MockCategoriesNotDeleted(1).First();
        childCategory.ParentCategoryId = parentCategory.Id;
        childCategory.ParentCategory = parentCategory;
        parentCategory.SubCategories = new List<Category> { childCategory };        

        _context.Categories.AddRange(parentCategory, childCategory);
        await _context.SaveChangesAsync();

        // Act
        var result = await _categoryRepository.GetByIdAsync(childCategory.Id);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(childCategory.Id);
        result.ParentCategory.Should().NotBeNull();
        result.ParentCategory.Id.Should().Be(parentCategory.Id);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnNull_WhenCategoryDoesNotExist()
    {
        // Arrange
        var nonExistentCategoryId = Guid.NewGuid();

        // Act
        var result = await _categoryRepository.GetByIdAsync(nonExistentCategoryId);

        // Assert
        result.Should().BeNull();
    }
}
