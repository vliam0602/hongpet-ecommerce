using HongPet.Application.Services.Commons;
using HongPet.Domain.Entities;
using HongPet.Domain.Repositories.Abstractions;
using HongPet.Domain.Test;
using HongPet.SharedViewModels.Generals;
using Moq;
using System.Linq.Expressions;

namespace HongPet.Application.Test.Services.Commons;
public class GenericServiceTest : SetupTest
{
    private readonly Mock<IGenericRepository<User>> _repositoryMock;
    private readonly GenericService<User> _service;

    // set up test data for each test
    public GenericServiceTest()
    {
        _repositoryMock = new Mock<IGenericRepository<User>>();
        _unitOfWorkMock.Setup(u => u.Repository<User>())
                       .Returns(_repositoryMock.Object);
        _service = new GenericService<User>(
                            _unitOfWorkMock.Object, 
                            _claimServiceMock.Object);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnEntity()
    {
        // Arrange
        var entity = MockUsers(1).First();
        _repositoryMock.Setup(r => r.GetByIdAsync(entity.Id)).ReturnsAsync(entity);

        // Act
        var result = await _service.GetByIdAsync(entity.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(entity.Id, result.Id);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllEntities()
    {
        // Arrange
        var entities = MockUsers(2);
        _repositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(entities);

        // Act
        var result = await _service.GetAllAsync();

        // Assert
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task AddAsync_ShouldAddEntity()
    {
        // Arrange
        var entity = MockUsers(1).First();

        // Act
        await _service.AddAsync(entity);

        // Assert
        _repositoryMock.Verify(r => r.AddAsync(entity), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateEntity()
    {
        // Arrange
        var entity = MockUsers(1).First();

        // Act
        await _service.UpdateAsync(entity);

        // Assert
        _repositoryMock.Verify(r => r.Update(entity), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_ShouldRemoveEntity()
    {
        // Arrange
        var entity = MockUsers(1).First();

        // Act
        await _service.DeleteAsync(entity.Id);

        // Assert
        _repositoryMock.Verify(r => r.DeleteAsync(entity.Id), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task GetAsync_ShouldReturnFilteredEntities()
    {
        // Arrange
        var entities = MockUsers(3);
        var entity1 = entities.First();
        Expression<Func<User, bool>> query = u => u.Fullname.Contains(entity1.Fullname);
        _repositoryMock.Setup(r => r.GetAsync(query))
                .ReturnsAsync(entities.Where(query.Compile()).ToList());

        // Act
        var result = await _service.GetAsync(query);

        // Assert
        Assert.Single(result);
        Assert.Contains(result, e => e.Fullname.Contains(entity1.Fullname));
    }

    [Theory]
    [InlineData(2, 2, 2)]
    [InlineData(3, 2, 1)]
    public async Task GetPagedAsync_ShouldReturnPagedEntities(int pageIndex, int pageSize, int expectedCount)
    {
        // Arrange
        var entities = MockUsers(5);
        var pagedList = new PagedList<User>(
            entities.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList(),
            entities.Count,
            pageIndex,
            pageSize
        );

        _repositoryMock.Setup(r => r.GetPagedAsync(pageIndex, pageSize, ""))
                       .ReturnsAsync(pagedList);

        // Act
        var result = await _service.GetPagedAsync(pageIndex, pageSize);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(entities.Count, result.TotalCount);
        Assert.Equal(expectedCount, result.Items.Count);
        Assert.Equal(pageIndex, result.CurrentPage);
        Assert.Equal((int)Math.Ceiling((double)entities.Count / pageSize), result.TotalPages);
    }

    [Fact]
    public async Task SoftDeleteAsync_ShouldMarkEntityAsDeleted()
    {
        // Arrange
        var entity = MockUsers(1).First();
        _repositoryMock.Setup(r => r.GetByIdAsync(entity.Id)).ReturnsAsync(entity);
        _claimServiceMock.Setup(c => c.GetCurrentUserId).Returns(Guid.NewGuid());

        // Act
        await _service.SoftDeleteAsync(entity.Id);

        // Assert
        _repositoryMock.Verify(r => r.GetByIdAsync(entity.Id), Times.Once);
        _repositoryMock.Verify(r => r.Update(It.Is<User>(e =>
            e.Id == entity.Id &&
            e.DeletedBy == _claimServiceMock.Object.GetCurrentUserId &&
            e.DeletedDate != null)), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task SoftDeleteAsync_ShouldThrowKeyNotFoundException_WhenEntityDoesNotExist()
    {
        // Arrange
        var nonExistentId = Guid.NewGuid();
        _repositoryMock.Setup(r => r.GetByIdAsync(nonExistentId)).ReturnsAsync((User?)null);

        // Act
        var act = async () => await _service.SoftDeleteAsync(nonExistentId);

        // Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(act);
        _repositoryMock.Verify(r => r.GetByIdAsync(nonExistentId), Times.Once);
        _repositoryMock.Verify(r => r.Update(It.IsAny<User>()), Times.Never);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Never);
    }

    [Fact]
    public async Task SoftDeleteAsync_ShouldThrowArgumentException_WhenEntityAlreadyDeleted()
    {
        // Arrange
        var entity = MockUsers(1).First();
        entity.DeletedDate = DateTime.UtcNow; // Mark as already deleted
        _repositoryMock.Setup(r => r.GetByIdAsync(entity.Id)).ReturnsAsync(entity);

        // Act
        var act = async () => await _service.SoftDeleteAsync(entity.Id);

        // Assert
        await Assert.ThrowsAsync<ArgumentException>(act);
        _repositoryMock.Verify(r => r.GetByIdAsync(entity.Id), Times.Once);
        _repositoryMock.Verify(r => r.Update(It.IsAny<User>()), Times.Never);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Never);
    }

}
