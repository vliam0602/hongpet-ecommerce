using HongPet.Domain.Entities;
using HongPet.Domain.Test;
using HongPet.Infrastructure.Database;
using HongPet.Infrastructure.Repositories.Commons;
using Microsoft.EntityFrameworkCore;

namespace HongPet.Infrastructure.Test.Repositories.Commons;

public class GenericRepositoryTest : SetupTest
{
    private readonly AppDbContext _context;
    public GenericRepositoryTest()
    {
        var _dbContextOptions = new DbContextOptionsBuilder<AppDbContext>()
                    .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                    .Options;
        _context = new AppDbContext(_dbContextOptions);
    }

    [Fact]
    public async Task AddAsync_ShouldAddEntity()
    {
        // Arrange
        var repository = new GenericRepository<User>(_context);
        var entity = MockUsers(1).First();

        // Act
        await repository.AddAsync(entity);
        await _context.SaveChangesAsync();

        // Assert
        var addedEntity = await _context.Set<User>().FindAsync(entity.Id);
        Assert.NotNull(addedEntity);
        Assert.Equal(entity.Fullname, addedEntity.Fullname);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnEntity()
    {
        // Arrange
        var repository = new GenericRepository<User>(_context);
        var entity = MockUsers(1).First();
        await _context.Set<User>().AddAsync(entity);
        await _context.SaveChangesAsync();

        // Act
        var retrievedEntity = await repository.GetByIdAsync(entity.Id);

        // Assert
        Assert.NotNull(retrievedEntity);
        Assert.Equal(entity.Fullname, retrievedEntity.Fullname);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllEntities()
    {
        // Arrange
        var repository = new GenericRepository<User>(_context);
        var entities = MockUsers(2);
        await _context.Set<User>().AddRangeAsync(entities);
        await _context.SaveChangesAsync();

        // Act
        var retrievedEntities = await repository.GetAllAsync();

        // Assert
        Assert.Equal(2, retrievedEntities.Count());
    }

    [Fact]
    public async Task Update_ShouldUpdateEntity()
    {
        // Arrange
        var repository = new GenericRepository<User>(_context);
        var entity = MockUsers(1).First();
        await _context.Set<User>().AddAsync(entity);
        await _context.SaveChangesAsync();

        // Act
        entity.Fullname = "Updated Entity";
        repository.Update(entity);
        await _context.SaveChangesAsync();

        // Assert
        var updatedEntity = await _context.Set<User>().FindAsync(entity.Id);
        Assert.NotNull(updatedEntity);
        Assert.Equal("Updated Entity", updatedEntity.Fullname);
    }

    [Fact]
    public async Task DeleteAsync_ShouldRemoveEntity()
    {
        // Arrange
        var repository = new GenericRepository<User>(_context);
        var entity = MockUsers(1).First();
        await _context.Set<User>().AddAsync(entity);
        await _context.SaveChangesAsync();

        // Act
        await repository.DeleteAsync(entity.Id);
        await _context.SaveChangesAsync();

        // Assert
        var deletedEntity = await _context.Set<User>().FindAsync(entity.Id);
        Assert.Null(deletedEntity);
    }

    [Fact]
    public async Task GetAsync_ShouldReturnFilteredEntities()
    {
        // Arrange
        var repository = new GenericRepository<User>(_context);
        var entities = MockUsers(3);
        await _context.Set<User>().AddRangeAsync(entities);
        await _context.SaveChangesAsync();

        // Act
        var entity1 = entities[0];
        var retrievedEntities = await repository.GetAsync(u => 
                u.Fullname.Contains(entity1.Fullname));

        // Assert
        Assert.Single(retrievedEntities);
        Assert.Contains(retrievedEntities, 
            e => e.Fullname.Contains(entity1.Fullname));
    }

    [Theory]
    [InlineData(2, 2, 2)]
    [InlineData(3, 2, 1)]
    public async Task GetPagedAsync_ShouldReturnPagedEntities(int pageIndex, 
        int pageSize, int expected)
    {
        // Arrange
        var repository = new GenericRepository<User>(_context);
        var entities = MockUsers(5);
        await _context.Set<User>().AddRangeAsync(entities);
        await _context.SaveChangesAsync();


        // Act
        var pagedResult = await repository.GetPagedAsync(pageIndex, pageSize);

        // Assert
        Assert.NotNull(pagedResult);
        Assert.Equal(entities.Count, pagedResult.TotalCount);
        Assert.Equal(expected, pagedResult.Items.Count);
        Assert.Equal(pageIndex, pagedResult.CurrentPage);
        Assert.Equal((int)Math.Ceiling((double)entities.Count / pageSize), pagedResult.TotalPages);
    }
}
