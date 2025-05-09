using AutoFixture;
using FluentAssertions;
using HongPet.Application.Services.Abstractions;
using HongPet.Domain.Test;
using HongPet.SharedViewModels.Generals;
using HongPet.SharedViewModels.Models;
using HongPet.SharedViewModels.ViewModels;
using HongPet.WebApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace HongPet.WebApi.Test.Controllers;

public class UsersControllerTest : SetupTest
{
    private readonly UsersController _usersController;
    private readonly Mock<IUserService> _userServiceMock;

    public UsersControllerTest()
    {
        // Mock IUserService
        _userServiceMock = _fixture.Freeze<Mock<IUserService>>();

        // Initialize UsersController
        _usersController = new UsersController(
            _fixture.Freeze<Mock<ILogger<UsersController>>>().Object,
            _userServiceMock.Object);
    }

    [Fact]
    public async Task GetUsers_ShouldReturnOk_WhenDataExists()
    {
        // Arrange
        var pagedUsers = _fixture.Create<PagedList<UserVM>>();
        var criteria = _fixture.Create<QueryListCriteria>();
        _userServiceMock.Setup(s => s.GetCustomersListAsync(
                                    criteria.PageIndex, criteria.PageSize, criteria.Keyword))
                        .ReturnsAsync(pagedUsers);

        // Act
        var result = await _usersController.GetUsers(criteria);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = result as OkObjectResult;
        okResult.Should().NotBeNull();
        okResult!.StatusCode.Should().Be(200); // Status code 200
        var response = okResult.Value as ApiResponse;
        response.Should().NotBeNull();
        response!.IsSuccess.Should().BeTrue();
        response.Data.Should().Be(pagedUsers);
    }

    [Fact]
    public async Task GetUsers_ShouldReturnInternalServerError_WhenExceptionIsThrown()
    {
        // Arrange
        var criteria = _fixture.Create<QueryListCriteria>();
        _userServiceMock.Setup(s => s.GetCustomersListAsync(
                                    criteria.PageIndex, criteria.PageSize, criteria.Keyword))
                        .ThrowsAsync(new Exception("Unexpected error"));

        // Act
        var result = await _usersController.GetUsers(criteria);

        // Assert
        result.Should().BeOfType<ObjectResult>();
        var objectResult = result as ObjectResult;
        objectResult.Should().NotBeNull();
        objectResult!.StatusCode.Should().Be(500); // Status code 500
        var response = objectResult.Value as ApiResponse;
        response.Should().NotBeNull();
        response!.IsSuccess.Should().BeFalse();
        response.ErrorMessage.Should().Contain("Unexpected error");
    }

    [Fact]
    public async Task GetUserDetail_ShouldReturnOk_WhenUserExists()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var userDetail = _fixture.Create<UserVM>();
        _userServiceMock.Setup(s => s.GetUserDetailAsync(userId)).ReturnsAsync(userDetail);

        // Act
        var result = await _usersController.GetUserDetail(userId);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = result as OkObjectResult;
        okResult.Should().NotBeNull();
        okResult!.StatusCode.Should().Be(200); // Status code 200
        var response = okResult.Value as ApiResponse;
        response.Should().NotBeNull();
        response!.IsSuccess.Should().BeTrue();
        response.Data.Should().Be(userDetail);
    }

    [Fact]
    public async Task GetUserDetail_ShouldReturnNotFound_WhenUserDoesNotExist()
    {
        // Arrange
        var userId = Guid.NewGuid();
        _userServiceMock.Setup(s => s.GetUserDetailAsync(userId))
                        .ThrowsAsync(new KeyNotFoundException("User not found"));

        // Act
        var result = await _usersController.GetUserDetail(userId);

        // Assert
        result.Should().BeOfType<NotFoundObjectResult>();
        var notFoundResult = result as NotFoundObjectResult;
        notFoundResult.Should().NotBeNull();
        notFoundResult!.StatusCode.Should().Be(404); // Status code 404
        var response = notFoundResult.Value as ApiResponse;
        response.Should().NotBeNull();
        response!.IsSuccess.Should().BeFalse();
        response.ErrorMessage.Should().Contain("User not found");
    }

    [Fact]
    public async Task GetUserDetail_ShouldReturnUnauthorized_WhenUnauthorizedAccessExceptionIsThrown()
    {
        // Arrange
        var userId = Guid.NewGuid();
        _userServiceMock.Setup(s => s.GetUserDetailAsync(userId))
                        .ThrowsAsync(new UnauthorizedAccessException("Unauthorized access"));

        // Act
        var result = await _usersController.GetUserDetail(userId);

        // Assert
        result.Should().BeOfType<UnauthorizedObjectResult>();
        var unauthorizedResult = result as UnauthorizedObjectResult;
        unauthorizedResult.Should().NotBeNull();
        unauthorizedResult!.StatusCode.Should().Be(401); // Status code 401
        var response = unauthorizedResult.Value as ApiResponse;
        response.Should().NotBeNull();
        response!.IsSuccess.Should().BeFalse();
        response.ErrorMessage.Should().Be("Unauthorized access");
    }

    [Fact]
    public async Task GetUserDetail_ShouldReturnInternalServerError_WhenExceptionIsThrown()
    {
        // Arrange
        var userId = Guid.NewGuid();
        _userServiceMock.Setup(s => s.GetUserDetailAsync(userId))
                        .ThrowsAsync(new Exception("Unexpected error"));

        // Act
        var result = await _usersController.GetUserDetail(userId);

        // Assert
        result.Should().BeOfType<ObjectResult>();
        var objectResult = result as ObjectResult;
        objectResult.Should().NotBeNull();
        objectResult!.StatusCode.Should().Be(500); // Status code 500
        var response = objectResult.Value as ApiResponse;
        response.Should().NotBeNull();
        response!.IsSuccess.Should().BeFalse();
        response.ErrorMessage.Should().Contain("Unexpected error");
    }

    [Fact]
    public async Task UpdateUser_ShouldReturnOk_WhenUserIsUpdated()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var userModel = _fixture.Create<UserUpdateModel>();
        var updatedUser = _fixture.Create<UserVM>();
        _userServiceMock.Setup(s => s.UpdateUserInfoAsync(userId, userModel)).ReturnsAsync(updatedUser);

        // Act
        var result = await _usersController.UpdateUser(userId, userModel);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = result as OkObjectResult;
        okResult.Should().NotBeNull();
        okResult!.StatusCode.Should().Be(200); // Status code 200
        var response = okResult.Value as ApiResponse;
        response.Should().NotBeNull();
        response!.IsSuccess.Should().BeTrue();
        response.Message.Should().Contain(userId.ToString());
        response.Data.Should().Be(updatedUser);
    }

    [Fact]
    public async Task UpdateUser_ShouldReturnNotFound_WhenUserDoesNotExist()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var userModel = _fixture.Create<UserUpdateModel>();
        _userServiceMock.Setup(s => s.UpdateUserInfoAsync(userId, userModel))
                        .ThrowsAsync(new KeyNotFoundException("User not found"));

        // Act
        var result = await _usersController.UpdateUser(userId, userModel);

        // Assert
        result.Should().BeOfType<NotFoundObjectResult>();
        var notFoundResult = result as NotFoundObjectResult;
        notFoundResult.Should().NotBeNull();
        notFoundResult!.StatusCode.Should().Be(404); // Status code 404
        var response = notFoundResult.Value as ApiResponse;
        response.Should().NotBeNull();
        response!.IsSuccess.Should().BeFalse();
        response.ErrorMessage.Should().Contain("User not found");
    }

    [Fact]
    public async Task UpdateUser_ShouldReturnUnauthorized_WhenUnauthorizedAccessExceptionIsThrown()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var userModel = _fixture.Create<UserUpdateModel>();
        _userServiceMock.Setup(s => s.UpdateUserInfoAsync(userId, userModel))
                        .ThrowsAsync(new UnauthorizedAccessException("Unauthorized access"));

        // Act
        var result = await _usersController.UpdateUser(userId, userModel);

        // Assert
        result.Should().BeOfType<UnauthorizedObjectResult>();
        var unauthorizedResult = result as UnauthorizedObjectResult;
        unauthorizedResult.Should().NotBeNull();
        unauthorizedResult!.StatusCode.Should().Be(401); // Status code 401
        var response = unauthorizedResult.Value as ApiResponse;
        response.Should().NotBeNull();
        response!.IsSuccess.Should().BeFalse();
        response.ErrorMessage.Should().Be("Unauthorized access");
    }

    [Fact]
    public async Task UpdateUser_ShouldReturnInternalServerError_WhenExceptionIsThrown()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var userModel = _fixture.Create<UserUpdateModel>();
        _userServiceMock.Setup(s => s.UpdateUserInfoAsync(userId, userModel))
                        .ThrowsAsync(new Exception("Unexpected error"));

        // Act
        var result = await _usersController.UpdateUser(userId, userModel);

        // Assert
        result.Should().BeOfType<ObjectResult>();
        var objectResult = result as ObjectResult;
        objectResult.Should().NotBeNull();
        objectResult!.StatusCode.Should().Be(500); // Status code 500
        var response = objectResult.Value as ApiResponse;
        response.Should().NotBeNull();
        response!.IsSuccess.Should().BeFalse();
        response.ErrorMessage.Should().Contain("Unexpected error");
    }
}
