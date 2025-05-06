using HongPet.Application.Services.Commons;
using HongPet.Domain.Enums;
using HongPet.Domain.Test;
using Microsoft.AspNetCore.Http;
using Moq;
using System.Security.Claims;
using Xunit;

namespace HongPet.Application.Test.Services.Commons
{
    public class ClaimServiceTest : SetupTest
    {
        private readonly Mock<IHttpContextAccessor> _httpContextAccessorMock;
        private readonly IClaimService _claimService;

        public ClaimServiceTest()
        {
            _httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            _claimService = new ClaimService(_httpContextAccessorMock.Object);
        }

        [Fact]
        public void GetCurrentUserId_ShouldReturnUserId_WhenClaimExists()
        {
            // Arrange
            var userId = Guid.NewGuid().ToString();
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userId)
            };
            var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(claims));
            var httpContext = new DefaultHttpContext { User = claimsPrincipal };
            _httpContextAccessorMock.Setup(x => x.HttpContext).Returns(httpContext);

            // Act
            var result = _claimService.GetCurrentUserId;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(Guid.Parse(userId), result);
        }

        [Fact]
        public void GetCurrentUserId_ShouldReturnNull_WhenClaimDoesNotExist()
        {
            // Arrange
            var httpContext = new DefaultHttpContext { User = new ClaimsPrincipal() };
            _httpContextAccessorMock.Setup(x => x.HttpContext).Returns(httpContext);

            // Act
            var result = _claimService.GetCurrentUserId;

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void GetCurrentEmail_ShouldReturnEmail_WhenClaimExists()
        {
            // Arrange
            var email = "test@example.com";
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, email)
            };
            var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(claims));
            var httpContext = new DefaultHttpContext { User = claimsPrincipal };
            _httpContextAccessorMock.Setup(x => x.HttpContext).Returns(httpContext);

            // Act
            var result = _claimService.GetCurrentEmail;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(email, result);
        }

        [Fact]
        public void GetCurrentRole_ShouldReturnRole_WhenClaimExists()
        {
            // Arrange
            var role = "Admin";
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Role, role)
            };
            var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(claims));
            var httpContext = new DefaultHttpContext { User = claimsPrincipal };
            _httpContextAccessorMock.Setup(x => x.HttpContext).Returns(httpContext);

            // Act
            var result = _claimService.GetCurrentRole;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(role, result);
        }

        [Fact]
        public void IsAdmin_ShouldReturnTrue_WhenRoleIsAdmin()
        {
            // Arrange
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Role, RoleEnum.Admin.ToString())
            };
            var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(claims));
            var httpContext = new DefaultHttpContext { User = claimsPrincipal };
            _httpContextAccessorMock.Setup(x => x.HttpContext).Returns(httpContext);

            // Act
            var result = _claimService.IsAdmin;

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsAdmin_ShouldReturnFalse_WhenRoleIsNotAdmin()
        {
            // Arrange
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Role, "User")
            };
            var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(claims));
            var httpContext = new DefaultHttpContext { User = claimsPrincipal };
            _httpContextAccessorMock.Setup(x => x.HttpContext).Returns(httpContext);

            // Act
            var result = _claimService.IsAdmin;

            // Assert
            Assert.False(result);
        }
    }
}
