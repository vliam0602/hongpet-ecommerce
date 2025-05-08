using AutoFixture;
using FluentAssertions;
using HongPet.Application.Services;
using HongPet.Domain.Entities;
using HongPet.Domain.Repositories.Abstractions;
using HongPet.Domain.Test;
using HongPet.SharedViewModels.Generals;
using HongPet.SharedViewModels.Models;
using HongPet.SharedViewModels.ViewModels;
using Moq;

namespace HongPet.Application.Test.Services;

public class ProductServiceTest : SetupTest
{
    private readonly ProductService _productService;
    private readonly Mock<IProductRepository> _productRepositoryMock;
    private readonly Mock<IGenericRepository<ProductAttribute>> _attributeRepositoryMock;
    private readonly Mock<ICategoryRepository> _categoryRepositoryMock;

    public ProductServiceTest()
    {
        // Mock the ProductRepository & setup in the UnitOfWork
        _productRepositoryMock = new Mock<IProductRepository>();
        _attributeRepositoryMock = new Mock<IGenericRepository<ProductAttribute>>();
        _categoryRepositoryMock = new Mock<ICategoryRepository>();

        _unitOfWorkMock.SetupGet(x => x.ProductRepository).Returns(_productRepositoryMock.Object);
        _unitOfWorkMock.SetupGet(x => x.CategoryRepository).Returns(_categoryRepositoryMock.Object);
        _unitOfWorkMock.Setup(x => x.Repository<ProductAttribute>()).Returns(_attributeRepositoryMock.Object);

        // Initialize the service with mocked dependencies
        _productService = new ProductService(
            _unitOfWorkMock.Object,
            _mapper,
            _claimServiceMock.Object);
    }

    [Fact]
    public async Task GetPagedProductAsync_ShouldReturnPagedProducts()
    {
        // Arrange
        var mockProducts = _fixture.Build<Product>().CreateMany(5).ToList();
        var pagedProducts = new PagedList<Product>(mockProducts, 5, 1, 10);

        _productRepositoryMock.Setup(x => x.GetPagedProductsAsync(1, 10, "", null))
                        .ReturnsAsync(pagedProducts);

        // Act
        var result = await _productService.GetPagedProductAsync(1, 10);

        // Assert
        var expectedOutput = _mapper.Map<PagedList<ProductGeneralVM>>(pagedProducts);

        Assert.NotNull(result);
        Assert.Equal(5, result.Items.Count);
        result.Should().BeEquivalentTo(expectedOutput);
        _productRepositoryMock.Verify(x => x.GetPagedProductsAsync(1, 10, "", null), Times.Once);
    }

    [Fact]
    public async Task GetProductDetailAsync_ShouldReturnProductDetail_WhenProductExists()
    {
        // Arrange
        var productId = Guid.NewGuid();
        var mockProduct = _fixture.Build<Product>()
                                  .With(p => p.Id, productId)
                                  .Without(p => p.DeletedDate)
                                  .Without(p => p.DeletedBy)
                                  .Create();

        _productRepositoryMock.Setup(x => x.GetProductDetailAsync(productId)).ReturnsAsync(mockProduct);

        // Act
        var result = await _productService.GetProductDetailAsync(productId);

        // Assert
        var expectedOutput = _mapper.Map<ProductDetailVM>(mockProduct);
        Assert.NotNull(result);
        Assert.Equal(productId, result.Id);
        result.Should().BeEquivalentTo(expectedOutput);
        _productRepositoryMock.Verify(x => x.GetProductDetailAsync(productId), Times.Once);
    }

    [Fact]
    public async Task GetProductDetailAsync_ShouldThrowKeyNotFoundException_WhenProductNotFound()
    {
        // Arrange
        var productId = Guid.NewGuid();

        _productRepositoryMock.Setup(x => x.GetProductDetailAsync(productId))
                              .ReturnsAsync((Product?)null);

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() =>
            _productService.GetProductDetailAsync(productId));
    }

    [Fact]
    public async Task SoftDeleteProductAsync_ShouldSoftDeleteProduct_WhenProductExists()
    {
        // Arrange
        var productId = Guid.NewGuid();
        var existingProduct = _fixture.Build<Product>()
                                  .With(p => p.Id, productId)
                                  .With(p => p.DeletedDate, () => null)
                                  .Without(p => p.DeletedBy)
                                  .Create();

        _productRepositoryMock.Setup(x => x.GetByIdAsync(productId)).ReturnsAsync(existingProduct);

        // Act
        await _productService.SoftDeleteProductAsync(productId);

        // Assert
        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(), Times.Once);
        _productRepositoryMock.Verify(x => x.Update(It.IsAny<Product>()), Times.Once);
        Assert.NotNull(existingProduct.DeletedDate);
    }

    [Fact]
    public async Task SoftDeleteProductAsync_ShouldThrowKeyNotFoundException_WhenProductNotFound()
    {
        // Arrange
        var productId = Guid.NewGuid();

        _productRepositoryMock.Setup(x => x.GetByIdAsync(productId)).ReturnsAsync((Product?)null);

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() =>
            _productService.SoftDeleteProductAsync(productId));
    }

    [Fact]
    public async Task AddProductAsync_ShouldAddProduct_WhenValid()
    {
        // Arrange
        var productModel = _fixture.Build<ProductModel>().Create();

        _categoryRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
                       .ReturnsAsync(_fixture.Build<Category>().Create());

        _productRepositoryMock.Setup(x => x.GetAttributeValuePairAsync(It.IsAny<string>(), It.IsAny<string>()))
                        .ReturnsAsync((ProductAttributeValue?)null);

        _productRepositoryMock.Setup(x => x.GetAttributeByNameAsync(It.IsAny<string>()))
                        .ReturnsAsync((ProductAttribute?)null);

        // Act
        var result = await _productService.AddProductAsync(productModel);

        // Assert
        Assert.NotEqual(Guid.Empty, result);
        _productRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Product>()), Times.Once);
        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task AddProductAsync_ShouldThrowKeyNotFoundException_WhenCategoryIdInvalid()
    {
        // Arrange
        var productModel = _fixture.Build<ProductModel>().Create();

        _categoryRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
                       .ReturnsAsync((Category?)null);        

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() =>
            _productService.AddProductAsync(productModel));
    }

    [Fact]
    public async Task UpdateProductAsync_ShouldUpdateProduct_WhenValid()
    {
        // Arrange
        var productId = Guid.NewGuid();
        var productModel = _fixture.Build<ProductModel>().Create();
        var existingProduct = _fixture.Build<Product>()
                                      .With(p => p.Id, productId)
                                      .Without(p => p.DeletedDate)
                                      .Without(p => p.DeletedBy)
                                      .Create();

        _productRepositoryMock.Setup(x => x.GetProductDetailAsync(productId)).ReturnsAsync(existingProduct);

        _unitOfWorkMock.Setup(x => x.CategoryRepository.GetByIdAsync(It.IsAny<Guid>()))
                       .ReturnsAsync(_fixture.Build<Category>().Create());

        _productRepositoryMock.Setup(x => x.GetAttributeValuePairAsync(It.IsAny<string>(), It.IsAny<string>()))
                        .ReturnsAsync((ProductAttributeValue?)null);

        _productRepositoryMock.Setup(x => x.GetAttributeByNameAsync(It.IsAny<string>()))
                        .ReturnsAsync((ProductAttribute?)null);

        // Act
        var result = await _productService.UpdateProductAsync(productId, productModel);

        // Assert
        var expectedOutput = _mapper.Map<ProductDetailVM>(existingProduct);
        Assert.NotNull(result);
        Assert.Equal(productId, result.Id);
        result.Should().BeEquivalentTo(expectedOutput);
        _productRepositoryMock.Verify(x => x.Update(It.IsAny<Product>()), Times.Once);
        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task UpdateProductAsync_ShouldThrowKeyNotFoundException_WhenProductNotFound()
    {
        // Arrange
        var productId = Guid.NewGuid();
        var productModel = _fixture.Build<ProductModel>().Create();

        _productRepositoryMock.Setup(x => x.GetProductDetailAsync(productId)).ReturnsAsync((Product?)null);

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() =>
            _productService.UpdateProductAsync(productId, productModel));
    }    

    [Fact]
    public async Task GetAllAttributes_ShouldReturnAllAttributes()
    {
        // Arrange
        var mockAttributes = _fixture.Build<ProductAttribute>().CreateMany(2).ToList();

        _attributeRepositoryMock.Setup(x => x.GetAllAsync()).ReturnsAsync(mockAttributes);

        // Act
        var result = await _productService.GetAllAttributes();

        // Assert
        var expectedOutput = _mapper.Map<List<AttributeVM>>(mockAttributes);
        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
        result.Should().BeEquivalentTo(expectedOutput);
        _attributeRepositoryMock.Verify(x => x.GetAllAsync(), Times.Once);
    }
}
