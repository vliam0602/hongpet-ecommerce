using AutoFixture;
using FluentAssertions;
using HongPet.Application.Commons;
using HongPet.Domain.Test;
using Microsoft.Extensions.Configuration;
using System.Text.Json;

namespace HongPet.Application.Test.Commons;

public class AppConfigurationMappingTest : SetupTest
{
    [Fact]
    public void AppConfiguration_ShouldMapCorrectly_FromJsonFile()
    {
        var appConfigData = _fixture.Build<AppConfiguration>().Create();
        var jsonConfig = JsonSerializer.Serialize(appConfigData, new JsonSerializerOptions
        {
            WriteIndented = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

        var configuration = new ConfigurationBuilder()
            .AddJsonStream(new MemoryStream(System.Text.Encoding.UTF8.GetBytes(jsonConfig)))
            .Build();

        // Act
        var appConfig = new AppConfiguration();
        configuration.Bind(appConfig);

        // Assert
        appConfig.Should().BeEquivalentTo(appConfigData);
    }
}

