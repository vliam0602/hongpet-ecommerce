//using FluentAssertions;
//using HongPet.Domain.Entities;
//using HongPet.Domain.Test;
//using HongPet.Infrastructure.Database;
//using HongPet.Infrastructure.Repositories;
//using Microsoft.EntityFrameworkCore;

//namespace HongPet.Infrastructure.Test.Repositories;

//public class ProductRepositoryTest : SetupTest
//{
//    private readonly ProductRepository _productRepository;
//    private readonly AppDbContext _context;

//    public ProductRepositoryTest()
//    {
//        _context = new AppDbContext(new DbContextOptionsBuilder<AppDbContext>()
//                    .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
//                    .Options);

//        // Reset the database
//        _context.Products.RemoveRange(_context.Products);
//        _context.SaveChanges();

//        // Initialize the product repository
//        _productRepository = new ProductRepository(_context);
//    }

//    [Fact]
//    public async Task GetAllAsync_ShouldReturnAllProducts_WhenDataExists()
//    {
//        // Arrange
//        var products = _fixture.Build<Product>()
//                               .Without(p => p.Variants)
//                               .CreateMany(10)
//                               .ToList();

//        _context.Products.AddRange(products);
//        await _context.SaveChangesAsync();

//        // Act
//        var result = await _productRepository.GetAllAsync();

//        // Assert
//        result.Should().NotBeNull();
//        result.Should().HaveCount(10);
//    }

//    [Fact]
//    public async Task GetPagedProductsAsync_ShouldReturnPagedProducts_WhenDataExists()
//    {
//        // Arrange
//        var products = _fixture.Build<Product>()
//                               .Without(p => p.Variants)
//                               .Without(p => p.Categories)
//                               .CreateMany(15)
//                               .ToList();

//        _context.Products.AddRange(products);
//        await _context.SaveChangesAsync();

//        // Act
//        var result = await _productRepository.GetPagedProductsAsync(pageIndex: 1, pageSize: 10);

//        // Assert
//        result.Should().NotBeNull();
//        result.Items.Should().HaveCount(10);
//        result.TotalCount.Should().Be(15);
//        result.CurrentPage.Should().Be(1);
//        result.TotalPages.Should().Be(2);
//    }

//    [Fact]
//    public async Task GetPagedProductsAsync_ShouldFilterByKeyword_WhenKeywordIsProvided()
//    {
//        // Arrange
//        var products = _fixture.Build<Product>()
//                               .Without(p => p.Variants)
//                               .Without(p => p.Categories)
//                               .CreateMany(10)
//                               .ToList();

//        products[0].Name = "TestProduct";

//        _context.Products.AddRange(products);
//        await _context.SaveChangesAsync();

//        // Act
//        var result = await _productRepository.GetPagedProductsAsync(pageIndex: 1, pageSize: 10, keyword: "Test");

//        // Assert
//        result.Should().NotBeNull();
//        result.Items.Should().HaveCount(1);
//        result.Items.First().Name.Should().Be("TestProduct");
//    }

//    [Fact]
//    public async Task GetProductDetailAsync_ShouldReturnProductDetail_WhenProductExists()
//    {
//        // Arrange
//        var product = _fixture.Build<Product>()
//                              .Without(p => p.Variants)
//                              .Without(p => p.Categories)
//                              .Without(p => p.Reviews)
//                              .Without(p => p.Images)
//                              .Create();

//        _context.Products.Add(product);
//        await _context.SaveChangesAsync();

//        // Act
//        var result = await _productRepository.GetProductDetailAsync(product.Id);

//        // Assert
//        result.Should().NotBeNull();
//        result.Id.Should().Be(product.Id);
//    }

//    [Fact]
//    public async Task GetProductDetailAsync_ShouldReturnNull_WhenProductDoesNotExist()
//    {
//        // Arrange
//        var nonExistentProductId = Guid.NewGuid();

//        // Act
//        var result = await _productRepository.GetProductDetailAsync(nonExistentProductId);

//        // Assert
//        result.Should().BeNull();
//    }

//    [Fact]
//    public async Task GetProductsByCategoryAsync_ShouldReturnProducts_WhenCategoryExists()
//    {
//        // Arrange
//        var category = _fixture.Build<Category>().Create();
//        var products = _fixture.Build<Product>()
//                               .With(p => p.Categories, new List<Category> { category })
//                               .CreateMany(5)
//                               .ToList();

//        _context.Products.AddRange(products);
//        await _context.SaveChangesAsync();

//        // Act
//        var result = await _productRepository.GetProductsByCategoryAsync(category.Name, pageIndex: 1, pageSize: 10);

//        // Assert
//        result.Should().NotBeNull();
//        result.Items.Should().HaveCount(5);
//    }

//    [Fact]
//    public async Task GetAttributeValuePairAsync_ShouldReturnAttributeValue_WhenExists()
//    {
//        // Arrange
//        var attribute = _fixture.Build<ProductAttribute>().Create();
//        var attributeValue = _fixture.Build<ProductAttributeValue>()
//                                     .With(av => av.Attribute, attribute)
//                                     .With(av => av.Value, "TestValue")
//                                     .Create();

//        _context.ProductAttributes.Add(attribute);
//        _context.ProductAttributeValues.Add(attributeValue);
//        await _context.SaveChangesAsync();

//        // Act
//        var result = await _productRepository.GetAttributeValuePairAsync(attribute.Name, "TestValue");

//        // Assert
//        result.Should().NotBeNull();
//        result.Value.Should().Be("TestValue");
//        result.Attribute.Name.Should().Be(attribute.Name);
//    }

//    [Fact]
//    public async Task GetAttributeValuePairAsync_ShouldReturnNull_WhenAttributeValueDoesNotExist()
//    {
//        // Act
//        var result = await _productRepository.GetAttributeValuePairAsync("NonExistentAttribute", "NonExistentValue");

//        // Assert
//        result.Should().BeNull();
//    }

//    [Fact]
//    public async Task GetAttributeByNameAsync_ShouldReturnAttribute_WhenExists()
//    {
//        // Arrange
//        var attribute = _fixture.Build<ProductAttribute>()
//                                .With(a => a.Name, "TestAttribute")
//                                .Create();

//        _context.ProductAttributes.Add(attribute);
//        await _context.SaveChangesAsync();

//        // Act
//        var result = await _productRepository.GetAttributeByNameAsync("TestAttribute");

//        // Assert
//        result.Should().NotBeNull();
//        result.Name.Should().Be("TestAttribute");
//    }

//    [Fact]
//    public async Task GetAttributeByNameAsync_ShouldReturnNull_WhenAttributeDoesNotExist()
//    {
//        // Act
//        var result = await _productRepository.GetAttributeByNameAsync("NonExistentAttribute");

//        // Assert
//        result.Should().BeNull();
//    }
//}
