using HongPet.Domain.Entities;
using HongPet.Domain.Test;
using HongPet.Infrastructure.Database;
using HongPet.Infrastructure.Repositories.Commons;

namespace HongPet.Infrastructure.Test.Repositories.Commons;

public class GenericRepositoryTest : SetupTest
{
    public GenericRepositoryTest()
    {
        using var context = new AppDbContext(_dbContextOptions);
        ClearDatabase(context).GetAwaiter().GetResult();
    }

    private async Task ClearDatabase(AppDbContext context)
    {
        context.Users.RemoveRange(context.Users);
        await context.SaveChangesAsync();
    }

    [Fact]
    public async Task AddAsync_ShouldAddEntity()
    {
        // Arrange
        using var context = new AppDbContext(_dbContextOptions);
        var repository = new GenericRepository<User>(context);
        var entity = UsersMockData(1).First();

        // Act
        await repository.AddAsync(entity);
        await context.SaveChangesAsync();

        // Assert
        var addedEntity = await context.Set<User>().FindAsync(entity.Id);
        Assert.NotNull(addedEntity);
        Assert.Equal(entity.Fullname, addedEntity.Fullname);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnEntity()
    {
        // Arrange
        using var context = new AppDbContext(_dbContextOptions);
        var repository = new GenericRepository<User>(context);
        var entity = UsersMockData(1).First();
        await context.Set<User>().AddAsync(entity);
        await context.SaveChangesAsync();

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
        using var context = new AppDbContext(_dbContextOptions);
        var repository = new GenericRepository<User>(context);
        var entities = UsersMockData(2);
        await context.Set<User>().AddRangeAsync(entities);
        await context.SaveChangesAsync();

        // Act
        var retrievedEntities = await repository.GetAllAsync();

        // Assert
        Assert.Equal(2, retrievedEntities.Count());
    }

    [Fact]
    public async Task Update_ShouldUpdateEntity()
    {
        // Arrange
        using var context = new AppDbContext(_dbContextOptions);
        var repository = new GenericRepository<User>(context);
        var entity = UsersMockData(1).First();
        await context.Set<User>().AddAsync(entity);
        await context.SaveChangesAsync();

        // Act
        entity.Fullname = "Updated Entity";
        repository.Update(entity);
        await context.SaveChangesAsync();

        // Assert
        var updatedEntity = await context.Set<User>().FindAsync(entity.Id);
        Assert.NotNull(updatedEntity);
        Assert.Equal("Updated Entity", updatedEntity.Fullname);
    }

    [Fact]
    public async Task DeleteAsync_ShouldRemoveEntity()
    {
        // Arrange
        using var context = new AppDbContext(_dbContextOptions);
        var repository = new GenericRepository<User>(context);
        var entity = UsersMockData(1).First();
        await context.Set<User>().AddAsync(entity);
        await context.SaveChangesAsync();

        // Act
        await repository.DeleteAsync(entity.Id);
        await context.SaveChangesAsync();

        // Assert
        var deletedEntity = await context.Set<User>().FindAsync(entity.Id);
        Assert.Null(deletedEntity);
    }

    [Fact]
    public async Task GetAsync_ShouldReturnFilteredEntities()
    {
        // Arrange
        using var context = new AppDbContext(_dbContextOptions);
        var repository = new GenericRepository<User>(context);
        var entities = UsersMockData(3);
        await context.Set<User>().AddRangeAsync(entities);
        await context.SaveChangesAsync();

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
    public async Task GetPagedAsync_ShouldReturnPagedEntities(int pageIndex, int pageSize, 
        int expected)
    {
        // Arrange
        using var context = new AppDbContext(_dbContextOptions);
        var repository = new GenericRepository<User>(context);
        var entities = UsersMockData(5);
        await context.Set<User>().AddRangeAsync(entities);
        await context.SaveChangesAsync();

        // Act
        var (items, totalCount) = await repository.GetPagedAsync(pageIndex, pageSize);

        // Assert
        Assert.Equal(5, totalCount);
        Assert.Equal(expected, items.Count());
    }
}
