using AutoFixture;
using FluentAssertions;
using HongPet.Domain.Entities;
using HongPet.Infrastructure.Database;
using HongPet.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace HongPet.Domain.Test.Repositories;

public class ProductRepositoryTest : SetupTest
{
    private readonly ProductRepository _productRepository;
    private readonly AppDbContext _dbContext;

    public ProductRepositoryTest()
    {
        // Set up in-memory database
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _dbContext = new AppDbContext(options);
        
        // reset the database
        _dbContext.Products.RemoveRange(_dbContext.Products);
        _dbContext.Categories.RemoveRange(_dbContext.Categories);
        _dbContext.SaveChanges();

        // init the repository
        _productRepository = new ProductRepository(_dbContext);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllProducts()
    {
        // Arrange
        var products = _fixture.Build<Product>()
                               .Without(p => p.Variants)
                               .CreateMany(3)
                               .ToList();
        await _dbContext.Products.AddRangeAsync(products);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _productRepository.GetAllAsync();

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(3);
    }

    [Theory]
    [InlineData(1, 2, 2)]
    [InlineData(3, 2, 1)]
    public async Task GetPagedProductsAsync_ShouldReturnPagedProducts(
        int pageIndex, int pageSize, int expectedItemsCount)
    {
        // Arrange
        var products = _fixture.Build<Product>()
                               .Without(p => p.DeletedDate)
                               .Without(p => p.DeletedBy)
                               .CreateMany(5)
                               .ToList();
        await _dbContext.Products.AddRangeAsync(products);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _productRepository.GetPagedProductsAsync(pageIndex, pageSize);

        // Assert
        result.Should().NotBeNull();
        result.Items.Should().HaveCount(expectedItemsCount);
        result.TotalCount.Should().Be(5);
    }

    [Fact]
    public async Task GetPagedProductsAsync_ShouldFilterByCategoryAndKeyword()
    {
        #region Arrange
        var categories = _fixture.Build<Category>()
                                    .With(c => c.Name, "Dog")
                                    .Without(c => c.Products)
                                    .Without(c => c.DeletedDate)
                                    .Without(c => c.DeletedBy)
                                    .CreateMany(5)
                                    .ToList();
        for (int i = 0; i < 3; i++) // set fisrt 3 categories to "Cat"        
        {
            categories[i].Name = "Cat";
        }

        foreach (var category in categories)
        {
            var product = _fixture.Build<Product>()
                .With(p => p.Name, category.Name) // set product name to category name
                .With(p => p.Categories, new List<Category> { category })
                .With(p => p.CreatedDate, DateTime.Now)
                .Without(p => p.Variants)
                .Without(p => p.Images)
                .Without(p => p.Reviews)
                .Without(p => p.DeletedDate)
                .Without(p => p.DeletedBy)
                .Create();
            category.Products.Add(product);
        }


        await _dbContext.Categories.AddRangeAsync(categories);
        await _dbContext.SaveChangesAsync();

        var categoriesFilter = new List<string> { "Cat" };
        var keyword = "Cat";
        #endregion

        // Act
        var result = await _productRepository.GetPagedProductsAsync(1, 10, keyword, categoriesFilter);

        // Assert
        result.Should().NotBeNull();
        result.Items.Should().HaveCount(3);
        result.Items[2].Id.Should().Be(categories[0].Products.First().Id); //because of the OrderByDescending of CreatedDate
        result.TotalCount.Should().Be(3);
    }

    [Fact]
    public async Task GetProductDetailAsync_ShouldReturnProductWithDetails()
    {
        // Arrange
        var product = _fixture.Create<Product>();
        await _dbContext.Products.AddAsync(product);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _productRepository.GetProductDetailAsync(product.Id);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(product.Id);
    }

    [Fact]
    public async Task GetProductsByCategoryAsync_ShouldReturnProductsInCategory()
    {
        // Arrange        
        var product = _fixture.Build<Product>()
                              .Without(p => p.Categories)
                              .Create();

        var category = _fixture.Build<Category>()
                               .With(c => c.Products, new List<Product> { product})
                               .Without(c => c.DeletedDate)
                               .Without(c => c.DeletedBy)
                               .Create();

        product.Categories.Add(category);

        await _dbContext.Categories.AddAsync(category);
        await _dbContext.Products.AddAsync(product);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _productRepository.GetProductsByCategoryAsync(category.Name, 1, 10);

        // Assert
        result.Should().NotBeNull();
        result.Items.Should().HaveCount(1);
        result.Items.First().Id.Should().Be(product.Id);
    }

    [Fact]
    public async Task GetAttributeValuePairAsync_ShouldReturnAttributeValuePair()
    {
        // Arrange
        var attribute = _fixture.Create<ProductAttribute>();

        var attributeValue = _fixture.Build<ProductAttributeValue>()
                                     .With(av => av.Attribute, attribute)
                                     .Create();
        await _dbContext.ProductAttributes.AddAsync(attribute);
        await _dbContext.ProductAttributeValues.AddAsync(attributeValue);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _productRepository.GetAttributeValuePairAsync(attribute.Name, attributeValue.Value);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(attributeValue.Id);
    }

    [Fact]
    public async Task GetAttributeByNameAsync_ShouldReturnAttribute()
    {
        // Arrange
        var attribute = _fixture.Create<ProductAttribute>();
        await _dbContext.ProductAttributes.AddAsync(attribute);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _productRepository.GetAttributeByNameAsync(attribute.Name);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(attribute.Id);
    }


}
