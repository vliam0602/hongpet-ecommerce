using FluentAssertions;
using HongPet.Application.Commons;
using Xunit;

namespace HongPet.Application.Test.Commons
{
    public class HashPasswordStringTest
    {
        [Fact]
        public void Hash_ShouldReturnHashedPassword()
        {
            // Arrange
            var password = "MySecurePassword";

            // Act
            var hashedPassword = password.Hash();

            // Assert
            hashedPassword.Should().NotBeNullOrEmpty();
            hashedPassword.Should().NotBe(password); // Ensure the hashed password is different from the original
        }

        [Fact]
        public void Verify_ShouldReturnTrue_WhenPasswordMatches()
        {
            // Arrange
            var password = "MySecurePassword";
            var hashedPassword = password.Hash();

            // Act
            var isVerified = password.Verify(hashedPassword);

            // Assert
            isVerified.Should().BeTrue();
        }

        [Fact]
        public void Verify_ShouldReturnFalse_WhenPasswordDoesNotMatch()
        {
            // Arrange
            var password = "MySecurePassword";
            var hashedPassword = password.Hash();
            var wrongPassword = "WrongPassword";

            // Act
            var isVerified = wrongPassword.Verify(hashedPassword);

            // Assert
            isVerified.Should().BeFalse();
        }        
    }
}

