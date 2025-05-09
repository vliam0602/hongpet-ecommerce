using AutoFixture;
using FluentAssertions;
using HongPet.Application.Services.Abstractions;
using HongPet.Domain.Entities;
using HongPet.Domain.Enums;
using HongPet.Domain.Test;
using HongPet.SharedViewModels.Models;
using HongPet.WebApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace HongPet.WebApi.Test.Controllers;

public class AuthControllerTest : SetupTest
{
    private readonly AuthController _authController;
    private readonly Mock<IUserService> _userServiceMock;

    public AuthControllerTest()
    {
        // Mock IUserService
        _userServiceMock = _fixture.Freeze<Mock<IUserService>>();

        // Initialize AuthController
        _authController = new AuthController(
            _appConfiguration, _claimServiceMock.Object, _userServiceMock.Object);
    }

    [Fact]
    public async Task Login_ShouldReturnUnauthorized_WhenAccountIsNull()
    {
        // Arrange
        var loginModel = _fixture.Create<LoginModel>();
        _userServiceMock.Setup(s => s.GetByEmailAndPasswordAsync(
                                loginModel.Email, loginModel.Password))
                        .ReturnsAsync((User?)null);

        // Act
        var result = await _authController.Login(loginModel);

        // Assert
        result.Should().BeOfType<UnauthorizedObjectResult>();
        var unauthorizedResult = result as UnauthorizedObjectResult;
        unauthorizedResult.Should().NotBeNull();
        unauthorizedResult!.StatusCode.Should().Be(401); // Status code 401
        var response = unauthorizedResult.Value as ApiResponse;
        response.Should().NotBeNull();
        response!.IsSuccess.Should().BeFalse();
        response.ErrorMessage.Should().Be("Wrong username/password.");
    }

    [Fact]
    public async Task Login_ShouldReturnOk_WhenLoginIsSuccessful()
    {
        // Arrange
        var loginModel = _fixture.Create<LoginModel>();
        var user = _fixture.Create<User>();
        _userServiceMock.Setup(s => s.GetByEmailAndPasswordAsync(
                                loginModel.Email, loginModel.Password))
                        .ReturnsAsync(user);

        // Act
        var result = await _authController.Login(loginModel);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = result as OkObjectResult;
        okResult.Should().NotBeNull();
        okResult!.StatusCode.Should().Be(200); // Status code 200
        var response = okResult.Value as ApiResponse;
        response.Should().NotBeNull();
        response!.IsSuccess.Should().BeTrue();
        response.Message.Should().Be("Logged in successfully.");
        response.Data.Should().NotBeNull();
    }

    [Fact]
    public async Task Login_ShouldReturnUnauthorized_WhenUnauthorizedAccessExceptionIsThrown()
    {
        // Arrange
        var loginModel = _fixture.Create<LoginModel>();
        _userServiceMock.Setup(s => s.GetByEmailAndPasswordAsync(
                                    loginModel.Email, loginModel.Password))
                        .ThrowsAsync(new UnauthorizedAccessException("Unauthorized access"));

        // Act
        var result = await _authController.Login(loginModel);

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
    public async Task Login_ShouldReturnInternalServerError_WhenExceptionIsThrown()
    {
        // Arrange
        var loginModel = _fixture.Create<LoginModel>();
        _userServiceMock.Setup(s => s.GetByEmailAndPasswordAsync(
                                    loginModel.Email, loginModel.Password))
                        .ThrowsAsync(new Exception("Unexpected error"));

        // Act
        var result = await _authController.Login(loginModel);

        // Assert
        result.Should().BeOfType<ObjectResult>();
        var objectResult = result as ObjectResult;
        objectResult.Should().NotBeNull();
        objectResult!.StatusCode.Should().Be(500); // Status code 500
        var response = objectResult.Value as ApiResponse;
        response.Should().NotBeNull();
        response!.IsSuccess.Should().BeFalse();
        response.ErrorMessage.Should().Be("Unexpected error: Unexpected error");
    }

    [Fact]
    public async Task AdminLogin_ShouldReturnOk_WhenLoginIsSuccessful()
    {
        // Arrange
        var loginModel = _fixture.Create<LoginModel>();
        var user = _fixture.Build<User>().With(u => u.Role, RoleEnum.Admin).Create();
        _userServiceMock.Setup(s => s.GetByEmailAndPasswordAsync(
                                    loginModel.Email, loginModel.Password))
                        .ReturnsAsync(user);

        // Act
        var result = await _authController.AdminLogin(loginModel);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = result as OkObjectResult;
        okResult.Should().NotBeNull();
        okResult!.StatusCode.Should().Be(200); // Status code 200
        var response = okResult.Value as ApiResponse;
        response.Should().NotBeNull();
        response!.IsSuccess.Should().BeTrue();
        response.Message.Should().Be("Logged in successfully.");
        response.Data.Should().NotBeNull();
    }

    [Fact]
    public async Task AdminLogin_ShouldReturnUnauthorized_WhenAccountIsNull()
    {
        // Arrange
        var loginModel = _fixture.Create<LoginModel>();
        _userServiceMock.Setup(s => s.GetByEmailAndPasswordAsync(
                                    loginModel.Email, loginModel.Password))
                        .ReturnsAsync((User?)null);

        // Act
        var result = await _authController.AdminLogin(loginModel);

        // Assert
        result.Should().BeOfType<UnauthorizedObjectResult>();
        var unauthorizedResult = result as UnauthorizedObjectResult;
        unauthorizedResult.Should().NotBeNull();
        unauthorizedResult!.StatusCode.Should().Be(401); // Status code 401
        var response = unauthorizedResult.Value as ApiResponse;
        response.Should().NotBeNull();
        response!.IsSuccess.Should().BeFalse();
        response.ErrorMessage.Should().Be("Wrong username/password.");
    }

    [Fact]
    public async Task AdminLogin_ShouldReturnUnauthorized_WhenUserIsNotAdmin()
    {
        // Arrange
        var loginModel = _fixture.Create<LoginModel>();
        var user = _fixture.Build<User>().With(u => u.Role, RoleEnum.Customer).Create();
        _userServiceMock.Setup(s => s.GetByEmailAndPasswordAsync(
                                    loginModel.Email, loginModel.Password))
                        .ReturnsAsync(user);

        // Act
        var result = await _authController.AdminLogin(loginModel);

        // Assert
        result.Should().BeOfType<UnauthorizedObjectResult>();
        var unauthorizedResult = result as UnauthorizedObjectResult;
        unauthorizedResult.Should().NotBeNull();
        unauthorizedResult!.StatusCode.Should().Be(401); // Status code 401
        var response = unauthorizedResult.Value as ApiResponse;
        response.Should().NotBeNull();
        response!.IsSuccess.Should().BeFalse();
        response.ErrorMessage.Should().Be("Only admin can access this page.");
    }

    [Fact]
    public async Task AdminLogin_ShouldReturnUnauthorized_WhenThrownedUnauthorizedAccessException()
    {
        // Arrange
        var loginModel = _fixture.Create<LoginModel>();
        var errorMessage = "Unauthorized access";
        _userServiceMock.Setup(s => s.GetByEmailAndPasswordAsync(
                                    loginModel.Email, loginModel.Password))
                        .ThrowsAsync(new UnauthorizedAccessException(errorMessage));

        // Act
        var result = await _authController.AdminLogin(loginModel);

        // Assert
        result.Should().BeOfType<UnauthorizedObjectResult>();
        var unauthorizedResult = result as UnauthorizedObjectResult;
        unauthorizedResult.Should().NotBeNull();
        unauthorizedResult!.StatusCode.Should().Be(401); // Status code 401
        var response = unauthorizedResult.Value as ApiResponse;
        response.Should().NotBeNull();
        response!.IsSuccess.Should().BeFalse();
        response.ErrorMessage.Should().Be(errorMessage);
    }

    [Fact]
    public async Task AdminLogin_ShouldReturnInternalServerError_WhenExceptionIsThrown()
    {
        // Arrange
        var loginModel = _fixture.Create<LoginModel>();
        _userServiceMock.Setup(s => s.GetByEmailAndPasswordAsync(
                                loginModel.Email, loginModel.Password))
                        .ThrowsAsync(new Exception("Unexpected error"));

        // Act
        var result = await _authController.AdminLogin(loginModel);

        // Assert
        result.Should().BeOfType<ObjectResult>();
        var objectResult = result as ObjectResult;
        objectResult.Should().NotBeNull();
        objectResult!.StatusCode.Should().Be(500); // Status code 500
        var response = objectResult.Value as ApiResponse;
        response.Should().NotBeNull();
        response!.IsSuccess.Should().BeFalse();
        response.ErrorMessage.Should().Be("Unexpected error: Unexpected error");
    }

    [Fact]
    public async Task RefreshToken_ShouldReturnUnauthorized_WhenUserIdIsNull()
    {
        // Arrange
        _claimServiceMock.Setup(c => c.GetCurrentUserId).Returns((Guid?)null);

        // Act
        var result = await _authController.RefreshToken();

        // Assert
        result.Should().BeOfType<UnauthorizedObjectResult>();
        var unauthorizedResult = result as UnauthorizedObjectResult;
        unauthorizedResult.Should().NotBeNull();
        unauthorizedResult!.StatusCode.Should().Be(401); // Status code 401
        var response = unauthorizedResult.Value as ApiResponse;
        response.Should().NotBeNull();
        response!.IsSuccess.Should().BeFalse();
        response.ErrorMessage.Should().Be("Invalid refresh token");
    }

    [Fact]
    public async Task RefreshToken_ShouldReturnNotFound_WhenUserDoesNotExist()
    {
        // Arrange
        var userId = Guid.NewGuid();
        _claimServiceMock.Setup(c => c.GetCurrentUserId).Returns(userId);
        _userServiceMock.Setup(s => s.GetByIdAsync(userId)).ReturnsAsync((User?)null);

        // Act
        var result = await _authController.RefreshToken();

        // Assert
        result.Should().BeOfType<NotFoundObjectResult>();
        var notFoundResult = result as NotFoundObjectResult;
        notFoundResult.Should().NotBeNull();
        notFoundResult!.StatusCode.Should().Be(404); // Status code 404
        var response = notFoundResult.Value as ApiResponse;
        response.Should().NotBeNull();
        response!.IsSuccess.Should().BeFalse();
        response.ErrorMessage.Should().Be($"User with id {userId} not found.");
    }

    [Fact]
    public async Task RefreshToken_ShouldReturnOk_WhenRefreshTokenIsSuccessful()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var user = _fixture.Create<User>();
        _claimServiceMock.Setup(c => c.GetCurrentUserId).Returns(userId);
        _userServiceMock.Setup(s => s.GetByIdAsync(userId)).ReturnsAsync(user);

        // Act
        var result = await _authController.RefreshToken();

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = result as OkObjectResult;
        okResult.Should().NotBeNull();
        okResult!.StatusCode.Should().Be(200); // Status code 200
        var response = okResult.Value as ApiResponse;
        response.Should().NotBeNull();
        response!.IsSuccess.Should().BeTrue();
        response.Message.Should().Be("Refresh token successful");
        response.Data.Should().NotBeNull();
    }

    [Fact]
    public async Task RefreshToken_ShouldReturnInternalServerError_WhenExceptionIsThrown()
    {
        // Arrange
        _claimServiceMock.Setup(c => c.GetCurrentUserId).Throws(new Exception("Unexpected error"));

        // Act
        var result = await _authController.RefreshToken();

        // Assert
        result.Should().BeOfType<ObjectResult>();
        var objectResult = result as ObjectResult;
        objectResult.Should().NotBeNull();
        objectResult!.StatusCode.Should().Be(500); // Status code 500
        var response = objectResult.Value as ApiResponse;
        response.Should().NotBeNull();
        response!.IsSuccess.Should().BeFalse();
        response.ErrorMessage.Should().Be("Unexpected error: Unexpected error");
    }

    [Fact]
    public async Task Register_ShouldReturnCreated_WhenRegistrationIsSuccessful()
    {
        // Arrange
        var registerModel = _fixture.Create<RegisterModel>();

        // Act
        var result = await _authController.Register(registerModel);

        // Assert
        result.Should().BeOfType<CreatedAtActionResult>();
        var createdResult = result as CreatedAtActionResult;
        createdResult.Should().NotBeNull();
        createdResult!.StatusCode.Should().Be(201); // Status code 201
        var response = createdResult.Value as ApiResponse;
        response.Should().NotBeNull();
        response!.IsSuccess.Should().BeTrue();
        response.Message.Should().Be("Register successfully!");
        response.Data.Should().Be(registerModel);
    }

    [Fact]
    public async Task Register_ShouldReturnBadRequest_WhenArgumentExceptionIsThrown()
    {
        // Arrange
        var registerModel = _fixture.Create<RegisterModel>();
        _userServiceMock.Setup(s => s.CreateNewAccountAsync(
            registerModel.Fullname, registerModel.Email, registerModel.Password))
            .ThrowsAsync(new ArgumentException("Invalid data"));

        // Act
        var result = await _authController.Register(registerModel);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
        var badRequestResult = result as BadRequestObjectResult;
        badRequestResult.Should().NotBeNull();
        badRequestResult!.StatusCode.Should().Be(400); // Status code 400
        var response = badRequestResult.Value as ApiResponse;
        response.Should().NotBeNull();
        response!.IsSuccess.Should().BeFalse();
        response.ErrorMessage.Should().Be("Invalid data");
    }

    [Fact]
    public async Task Register_ShouldReturnInternalServerError_WhenExceptionIsThrown()
    {
        // Arrange
        var registerModel = _fixture.Create<RegisterModel>();
        _userServiceMock.Setup(s => s.CreateNewAccountAsync(
            registerModel.Fullname, registerModel.Email, registerModel.Password))
            .ThrowsAsync(new Exception("Unexpected error"));

        // Act
        var result = await _authController.Register(registerModel);

        // Assert
        result.Should().BeOfType<ObjectResult>();
        var objectResult = result as ObjectResult;
        objectResult.Should().NotBeNull();
        objectResult!.StatusCode.Should().Be(500); // Status code 500
        var response = objectResult.Value as ApiResponse;
        response.Should().NotBeNull();
        response!.IsSuccess.Should().BeFalse();
        response.ErrorMessage.Should().Be("Unexpected error: Unexpected error");
    }
}
