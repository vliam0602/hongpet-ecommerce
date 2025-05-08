using AutoFixture;
using FluentAssertions;
using HongPet.Domain.Entities;
using HongPet.Domain.Enums;
using HongPet.Domain.Test;
using HongPet.Infrastructure.Database;
using HongPet.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace HongPet.Infrastructure.Test.Repositories
{
    public class UserRepositoryTest : SetupTest
    {
        private readonly UserRepository _userRepository;
        private readonly AppDbContext _context;

        public UserRepositoryTest()
        {
            _context = new AppDbContext(new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options);

            // Reset the database
            _context.Users.RemoveRange(_context.Users);
            _context.SaveChanges();

            // Initialize the user repository
            _userRepository = new UserRepository(_context, _mapper);
        }

        [Fact]
        public async Task IsEmailExistAsync_ShouldReturnTrue_WhenEmailExists()
        {
            // Arrange
            var users = _fixture.Build<User>()
                                .Without(u => u.Orders)
                                .Without(u => u.Reviews)
                                .CreateMany(5)
                                .ToList();

            users[0].Email = "test@example.com";

            _context.Users.AddRange(users);
            await _context.SaveChangesAsync();

            // Act
            var result = await _userRepository.IsEmailExistAsync("test@example.com");

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public async Task IsEmailExistAsync_ShouldReturnFalse_WhenEmailDoesNotExist()
        {
            // Arrange
            var users = _fixture.Build<User>()
                                .Without(u => u.Orders)
                                .Without(u => u.Reviews)
                                .CreateMany(5)
                                .ToList();

            _context.Users.AddRange(users);
            await _context.SaveChangesAsync();

            // Act
            var result = await _userRepository.IsEmailExistAsync("nonexistent@example.com");

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public async Task GetCustomersPagedAsync_ShouldReturnPagedList_WhenCustomersExist()
        {
            // Arrange
            var users = _fixture.Build<User>()
                                .Without(u => u.Orders)
                                .Without(u => u.Reviews)
                                .CreateMany(6)
                                .ToList();

            users[0].Role = RoleEnum.Admin; // Admin user should not be included
            _context.Users.AddRange(users);
            await _context.SaveChangesAsync();

            // Act
            var result = await _userRepository.GetCustomersPagedAsync(
                pageIndex: 2, pageSize: 3);

            // Assert
            result.Should().NotBeNull();
            result.Items.Should().HaveCount(2); // Only customers are included
            result.TotalCount.Should().Be(5); // Exclude the admin user
            result.CurrentPage.Should().Be(2);
            result.TotalPages.Should().Be(2);
        }

        [Fact]
        public async Task GetCustomersPagedAsync_ShouldFilterByKeyword_WhenKeywordIsProvided()
        {
            // Arrange
            var users = _fixture.Build<User>()
                                .Without(u => u.Orders)
                                .Without(u => u.Reviews)
                                .CreateMany(10)
                                .ToList();

            users[0].Fullname = "Test User";

            _context.Users.AddRange(users);
            await _context.SaveChangesAsync();

            // Act
            var result = await _userRepository.GetCustomersPagedAsync(pageIndex: 1, pageSize: 10, keyword: "Test");

            // Assert
            result.Should().NotBeNull();
            result.Items.Should().HaveCount(1);
            result.Items.First().Fullname.Should().Be("Test User");
        }
    }
}
