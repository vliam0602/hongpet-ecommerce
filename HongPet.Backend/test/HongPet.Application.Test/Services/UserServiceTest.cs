using AutoFixture;
using FluentAssertions;
using HongPet.Application.Commons;
using HongPet.Application.Services;
using HongPet.Domain.DTOs;
using HongPet.Domain.Entities;
using HongPet.Domain.Repositories.Abstractions;
using HongPet.Domain.Test;
using HongPet.SharedViewModels.Generals;
using HongPet.SharedViewModels.Models;
using HongPet.SharedViewModels.ViewModels;
using Moq;
using System.Linq.Expressions;

namespace HongPet.Application.Test.Services;

public class UserServiceTest : SetupTest
{
    private readonly UserService _userService;
    private readonly Mock<IUserRepository> _userRepositoryMock;

    public UserServiceTest()
    {
        // Mock the UserRepository
        _userRepositoryMock = new Mock<IUserRepository>();
        _unitOfWorkMock.SetupGet(x => x.UserRepository).Returns(_userRepositoryMock.Object);

        // Initialize the service with mocked dependencies
        _userService = new UserService(
            _unitOfWorkMock.Object,
            _claimServiceMock.Object,
            _mapper);
    }

    [Fact]
    public async Task CreateNewAccountAsync_ShouldCreateAccount_WhenEmailDoesNotExist()
    {
        // Arrange
        var email = "test@example.com";
        var fullname = "Test User";
        var password = "password123";

        _userRepositoryMock.Setup(x => x.IsEmailExistAsync(email)).ReturnsAsync(false);

        // Act
        await _userService.CreateNewAccountAsync(fullname, email, password);

        // Assert
        _userRepositoryMock.Verify(x => x.AddAsync(It.IsAny<User>()), Times.Once);
        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task CreateNewAccountAsync_ShouldThrowArgumentException_WhenEmailExists()
    {
        // Arrange
        var email = "test@example.com";
        var fullname = "Test User";
        var password = "password123";

        _userRepositoryMock.Setup(x => x.IsEmailExistAsync(email)).ReturnsAsync(true);

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() =>
            _userService.CreateNewAccountAsync(fullname, email, password));
    }

    [Fact]
    public async Task GetByEmailAndPasswordAsync_ShouldReturnUser_WhenCredentialsAreValid()
    {
        // Arrange
        var email = "test@example.com";
        var password = "password123";
        var mockUser = _fixture.Build<User>()
                               .With(u => u.Email, email)
                               .With(u => u.Password, password.Hash())
                               .Without(u => u.DeletedDate)
                               .Without(u => u.DeletedBy)
                               .Create();

        _userRepositoryMock.Setup(x => x.GetAsync(It.IsAny<Expression<Func<User, bool>>>()))
                           .ReturnsAsync(new List<User> { mockUser });

        // Act
        var result = await _userService.GetByEmailAndPasswordAsync(email, password);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(email, result.Email);
    }

    [Fact]
    public async Task GetByEmailAndPasswordAsync_ShouldThrowUnauthorizedAccessException_WhenAccountIsInactivated()
    {
        // Arrange
        var email = "test@example.com";
        var password = "password123";

        var mockUser = _fixture.Build<User>()
                               .With(u => u.Email, email)
                               .With(u => u.Password, password.Hash())
                               .Create();

        _userRepositoryMock.Setup(x => x.GetAsync(It.IsAny<Expression<Func<User, bool>>>()))
                           .ReturnsAsync(new List<User> { mockUser });

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() =>
            _userService.GetByEmailAndPasswordAsync(email, password));
    }

    [Fact]
    public async Task GetByEmailAndPasswordAsync_ShouldReturnNull_WhenCredentialsAreInvalid()
    {
        // Arrange
        var email = "test@example.com";
        var password = "password123";

        _userRepositoryMock.Setup(x => x.GetAsync(It.IsAny<Expression<Func<User, bool>>>()))
                           .ReturnsAsync(new List<User>());

        // Act
        var result = await _userService.GetByEmailAndPasswordAsync(email, password);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task ChangePasswordAsync_ShouldChangePassword_WhenOldPasswordIsCorrect()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var oldPassword = "oldPassword123";
        var newPassword = "newPassword123";
        var mockUser = _fixture.Build<User>()
                               .With(u => u.Id, userId)
                               .With(u => u.Password, oldPassword.Hash())
                               .Without(u => u.DeletedDate)
                               .Without(u => u.DeletedBy)
                               .Create();

        _claimServiceMock.SetupGet(x => x.GetCurrentUserId).Returns(userId);
        _userRepositoryMock.Setup(x => x.GetByIdAsync(userId)).ReturnsAsync(mockUser);

        // Act
        var result = await _userService.ChangePasswordAsync(userId, oldPassword, newPassword);

        // Assert
        Assert.True(result);
        _userRepositoryMock.Verify(x => x.Update(It.IsAny<User>()), Times.Once);
        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task ChangePasswordAsync_ShouldThrowArgumentException_WhenOldPasswordIsIncorrect()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var oldPassword = "oldPassword123";
        var newPassword = "newPassword123";
        var mockUser = _fixture.Build<User>()
                               .With(u => u.Id, userId)
                               .With(u => u.Password, "differentPassword".Hash())
                               .Without(u => u.DeletedDate)
                               .Without(u => u.DeletedBy)
                               .Create();

        _claimServiceMock.SetupGet(x => x.GetCurrentUserId).Returns(userId);
        _userRepositoryMock.Setup(x => x.GetByIdAsync(userId)).ReturnsAsync(mockUser);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ArgumentException>(() =>
            _userService.ChangePasswordAsync(userId, oldPassword, newPassword));
        Assert.Equal("Old password is incorrect.", exception.Message);
    }

    [Fact]
    public async Task GetUserDetailAsync_ShouldReturnUserDetail_WhenUserExists()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var mockUser = _fixture.Build<User>()
                               .With(u => u.Id, userId)
                               .Without(u => u.DeletedDate)
                               .Without(u => u.DeletedBy)
                               .Create();

        _claimServiceMock.SetupGet(x => x.GetCurrentUserId).Returns(userId);
        _userRepositoryMock.Setup(x => x.GetByIdAsync(userId)).ReturnsAsync(mockUser);

        // Act
        var result = await _userService.GetUserDetailAsync(userId);

        // Assert
        var expectedOutput = _mapper.Map<UserVM>(mockUser);
        Assert.NotNull(result);
        Assert.Equal(userId, result.Id);
        result.Should().BeEquivalentTo(expectedOutput);
    }

    [Fact]
    public async Task GetUserDetailAsync_ShouldThrowKeyNotFoundException_WhenUserDoesNotExist()
    {
        // Arrange
        var userId = Guid.NewGuid();

        _claimServiceMock.SetupGet(x => x.GetCurrentUserId).Returns(userId);
        _userRepositoryMock.Setup(x => x.GetByIdAsync(userId)).ReturnsAsync((User?)null);

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() =>
            _userService.GetUserDetailAsync(userId));
    }

    [Fact]
    public async Task GetCustomersListAsync_ShouldReturnPagedCustomers()
    {
        // Arrange
        var mockUsers = _fixture.Build<UserDto>().CreateMany(5).ToList();
        var pagedUsers = new PagedList<UserDto>(mockUsers, 5, 1, 10);

        _claimServiceMock.SetupGet(x => x.IsAdmin).Returns(true);
        _userRepositoryMock.Setup(x => x.GetCustomersPagedAsync(1, 10, ""))
                           .ReturnsAsync(pagedUsers);

        // Act
        var result = await _userService.GetCustomersListAsync(1, 10);

        // Assert
        var expectedOutput = _mapper.Map<PagedList<UserVM>>(pagedUsers);
        Assert.NotNull(result);
        Assert.Equal(5, result.Items.Count);
        result.Should().BeEquivalentTo(expectedOutput);
    }

    [Fact]
    public async Task InactiveUserAsync_ShouldSoftDeleteUser_WhenUserExists()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var mockUser = _fixture.Build<User>()
                               .With(u => u.Id, userId)
                               .With(u => u.DeletedDate, () => null)
                               .Without(u => u.DeletedBy)
                               .Create();

        _claimServiceMock.SetupGet(x => x.IsAdmin).Returns(true);
        _userRepositoryMock.Setup(x => x.GetByIdAsync(userId)).ReturnsAsync(mockUser);

        // Act
        await _userService.InactiveUserAsync(userId);

        // Assert
        _userRepositoryMock.Verify(x => x.Update(It.IsAny<User>()), Times.Once);
        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(), Times.Once);
        mockUser.DeletedDate.Should().NotBeNull();
    }

    [Fact]
    public async Task UpdateAvatarAsync_ShouldUpdateAvatar_WhenUserExists()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var avatarUrl = "https://example.com/avatar.jpg";
        var mockUser = _fixture.Build<User>()
                               .With(u => u.Id, userId)
                               .Without(u => u.DeletedDate)
                               .Without(u => u.DeletedBy)
                               .Create();

        _claimServiceMock.SetupGet(x => x.GetCurrentUserId).Returns(userId);
        _userRepositoryMock.Setup(x => x.GetByIdAsync(userId)).ReturnsAsync(mockUser);

        // Act
        var result = await _userService.UpdateAvatarAsync(userId, avatarUrl);

        // Assert
        Assert.True(result);
        Assert.Equal(avatarUrl, mockUser.AvatarUrl);
        _userRepositoryMock.Verify(x => x.Update(It.IsAny<User>()), Times.Once);
        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task UpdateUserInfoAsync_ShouldUpdateUserInfo_WhenUserExists()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var userModel = _fixture.Build<UserUpdateModel>().Create();
        var mockUser = _fixture.Build<User>()
                               .With(u => u.Id, userId)
                               .Without(u => u.DeletedDate)
                               .Without(u => u.DeletedBy)
                               .Create();

        _claimServiceMock.SetupGet(x => x.GetCurrentUserId).Returns(userId);
        _userRepositoryMock.Setup(x => x.GetByIdAsync(userId)).ReturnsAsync(mockUser);

        // Act
        var result = await _userService.UpdateUserInfoAsync(userId, userModel);

        // Assert
        _mapper.Map(userModel, mockUser);
        var expectedOutput = _mapper.Map<UserVM>(mockUser);
        Assert.NotNull(result);
        Assert.Equal(userId, result.Id);
        _userRepositoryMock.Verify(x => x.Update(It.IsAny<User>()), Times.Once);
        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(), Times.Once);
    }
}
