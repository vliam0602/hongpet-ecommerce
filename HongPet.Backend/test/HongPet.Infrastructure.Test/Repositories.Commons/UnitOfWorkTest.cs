﻿//// Comment this test class because
//// logic of the UnitOfWork is still on going

//using HongPet.Domain.Entities;
//using HongPet.Domain.Repositories.Abstractions;
//using HongPet.Domain.Test;
//using HongPet.Infrastructure.Database;
//using HongPet.Infrastructure.Repositories.Commons;
//using Microsoft.EntityFrameworkCore;
//using Moq;

//namespace HongPet.Infrastructure.Test.Repositories.Commons;
//public class UnitOfWorkTest : SetupTest
//{
//    private readonly AppDbContext  _context;
//    private readonly UnitOfWork _unitOfWork;

//    public UnitOfWorkTest()
//    {
//        var options = new DbContextOptionsBuilder<AppDbContext>()
//            .UseInMemoryDatabase(databaseName: "TestDatabase")
//            .Options;
//        var productRepositoryMock = new Mock<IProductRepository>();
//        var userTokenRepositoryMock = new Mock<IUserTokenRepository>();
//        _context = new AppDbContext(_dbContextOptions);
//        _unitOfWork = new UnitOfWork(_context, 
//                                        productRepositoryMock.Object,
//                                        userTokenRepositoryMock.Object);
//    }

//    [Fact]
//    public void Repository_ShouldReturnGenericRepository()
//    {
//        // Act
//        var userRepository = _unitOfWork.Repository<User>();

//        // Assert
//        Assert.NotNull(userRepository);
//        Assert.IsType<GenericRepository<User>>(userRepository);
//    }

//    [Fact]
//    public async Task SaveChangesAsync_ShouldSaveChanges()
//    {
//        // Arrange
//        var userRepository = _unitOfWork.Repository<User>();
//        var user = UsersMockData(1).First();
//        await userRepository.AddAsync(user);

//        // Act
//        var result = await _unitOfWork.SaveChangesAsync();

//        // Assert
//        Assert.Equal(1, result);
//    }

//    [Fact]
//    public void Dispose_ShouldDisposeContext()
//    {
//        // Act
//        _unitOfWork.Dispose();

//        // Assert
//        Assert.Throws<ObjectDisposedException>(() => _context.Users.ToList());
//    }
//}
