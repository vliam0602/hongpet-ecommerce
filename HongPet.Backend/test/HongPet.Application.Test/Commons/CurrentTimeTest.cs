using FluentAssertions;
using HongPet.Application.Commons;
using Xunit;

namespace HongPet.Application.Test.Commons
{
    public class CurrentTimeTest
    {
        [Fact]
        public void GetCurrentTime_ShouldReturnCurrentLocalTime()
        {
            // Act
            var currentTime = CurrentTime.GetCurrentTime;

            // Assert
            currentTime.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(1));
        }
    }
}

