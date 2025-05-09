using FluentAssertions;
using HongPet.Application.Commons;
using HongPet.Domain.Entities;
using HongPet.Domain.Enums;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace HongPet.Application.Test.Commons;

public class GenerateJwtStringTest
{
    [Fact]
    public void GenerateJwt_ShouldReturnValidJwtToken_WhenInputIsValid()
    {
        // Arrange
        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = "test@example.com",
            Role = RoleEnum.Admin
        };
        var secretKey = "ThisIsASecretKeyForTestingPurposesOnly!";
        var expDate = DateTime.UtcNow.AddHours(1);

        // Act
        var token = user.GenerateJwt(secretKey, expDate);

        // Assert
        token.Should().NotBeNullOrEmpty();

        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token);

        jwtToken.Should().NotBeNull();
        jwtToken.Claims.Should().Contain(c => c.Type == ClaimTypes.NameIdentifier && c.Value == user.Id.ToString());
        jwtToken.Claims.Should().Contain(c => c.Type == ClaimTypes.Email && c.Value == user.Email);
        jwtToken.Claims.Should().Contain(c => c.Type == ClaimTypes.Role && c.Value == user.Role.ToString());
        jwtToken.ValidTo.Should().BeCloseTo(expDate, TimeSpan.FromSeconds(1));
    }

    [Fact]
    public void GenerateJwt_ShouldThrowException_WhenSecretKeyIsInvalid()
    {
        // Arrange
        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = "test@example.com",
            Role = RoleEnum.Admin
        };
        var secretKey = ""; // Invalid secret key
        var expDate = DateTime.UtcNow.AddHours(1);

        // Act & Assert
        Assert.Throws<ArgumentException>(() => user.GenerateJwt(secretKey, expDate));
    }

    [Fact]
    public void GenerateJwt_ShouldIncludeCorrectExpirationDate()
    {
        // Arrange
        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = "test@example.com",
            Role = RoleEnum.Customer
        };
        var secretKey = "ThisIsASecretKeyForTestingPurposesOnly!";
        var expDate = DateTime.UtcNow.AddMinutes(30);

        // Act
        var token = user.GenerateJwt(secretKey, expDate);

        // Assert
        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token);

        jwtToken.ValidTo.Should().BeCloseTo(expDate, TimeSpan.FromSeconds(1));
    }
}
