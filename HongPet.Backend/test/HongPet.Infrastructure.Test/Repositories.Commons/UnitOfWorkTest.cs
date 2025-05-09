using FluentAssertions;
using HongPet.Domain.Entities;
using HongPet.Domain.Repositories.Abstractions;
using HongPet.Domain.Test;
using HongPet.Infrastructure.Database;
using HongPet.Infrastructure.Repositories.Commons;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Xml.Linq;

namespace HongPet.Infrastructure.Test.Repositories.Commons;
public class UnitOfWorkTest : SetupTest
{
    private readonly AppDbContext _context;
    private readonly UnitOfWork _unitOfWork;
    private readonly Mock<IProductRepository> productRepositoryMock;
    private readonly Mock<IUserRepository> userRepositoryMock;
    private readonly Mock<IReviewRepository> reviewRepositoryMock;
    private readonly Mock<IOrderRepository> orderRepositoryMock;
    private readonly Mock<ICategoryRepository> categoryRepositoryMock;

    public UnitOfWorkTest()
    {
        // mock the dependencies
        var options = new DbContextOptionsBuilder<AppDbContext>()
                    .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                    .Options;
        _context = new AppDbContext(options);
        productRepositoryMock = new Mock<IProductRepository>();
        userRepositoryMock = new Mock<IUserRepository>();
        reviewRepositoryMock = new Mock<IReviewRepository>();
        orderRepositoryMock = new Mock<IOrderRepository>();
        categoryRepositoryMock = new Mock<ICategoryRepository>();

        // create the unit of work with mocked dependencies
        _unitOfWork = new UnitOfWork(
            _context,
            productRepositoryMock.Object,
            userRepositoryMock.Object,
            reviewRepositoryMock.Object,
            orderRepositoryMock.Object,
            categoryRepositoryMock.Object);
    }

    [Fact]
    public void Repository_ShouldReturnGenericRepository()
    {
        // Act
        var userRepository = _unitOfWork.Repository<User>();

        // Assert
        Assert.NotNull(userRepository);
        Assert.IsType<GenericRepository<User>>(userRepository);
    }

    [Fact]
    public async Task SaveChangesAsync_ShouldSaveChanges()
    {
        // Arrange
        var userRepository = _unitOfWork.Repository<User>();
        var user = MockUsers(1).First();
        await userRepository.AddAsync(user);

        // Act
        var result = await _unitOfWork.SaveChangesAsync();

        // Assert
        Assert.Equal(1, result);
    }

    [Fact]
    public void Dispose_ShouldDisposeContext()
    {
        // Act
        _unitOfWork.Dispose();

        // Assert
        Assert.Throws<ObjectDisposedException>(() => _context.Users.ToList());
    }    

    [Fact]
    public void Repository_ShouldCreateAndReturnNewRepository_WhenRepositoryDoesNotExist()
    {
        // Act
        var productRepository = _unitOfWork.Repository<Product>();

        // Assert
        productRepository.Should().NotBeNull();
        productRepository.Should().BeOfType<GenericRepository<Product>>(); // Ensure the correct type is created
    }

    [Fact]
    public void Repository_ShouldReturnExistingRepository_WhenRepositoryAlreadyExists()
    {
        // Arrange
        var userRepository = _unitOfWork.Repository<User>();

        // Act
        var retrievedRepository = _unitOfWork.Repository<User>();

        // Assert
        retrievedRepository.Should().NotBeNull();
        retrievedRepository.Should().BeSameAs(userRepository); // Ensure the same instance is returned
    }

}
