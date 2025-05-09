using AutoFixture;
using FluentAssertions;
using HongPet.Application.Heplers;
using HongPet.Domain.Test;

namespace HongPet.Application.Test.Heplers;

public class JsonHelperTest : SetupTest
{
    [Fact]
    public void LoadDataFromJson_ShouldReturnCorrectData_WhenJsonIsValid()
    {
        // Arrange
        var testData = _fixture.CreateMany<TestItem>(5).ToList(); 
        var jsonContent = Newtonsoft.Json.JsonConvert.SerializeObject(testData);

        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "testData.json");
        File.WriteAllText(filePath, jsonContent);

        // Act
        var result = JsonHelper.LoadDataFromJson<TestItem>(filePath);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(testData.Count);
        result.Should().BeEquivalentTo(testData);

        // Clean up
        File.Delete(filePath);
    }

    [Fact]
    public void LoadDataFromJson_ShouldThrowException_WhenJsonIsInvalid()
    {
        // Arrange
        // JSON không hợp lệ
        var invalidJsonContent = @"{ ""Id"": ""00000000-0000-0000-0000-000000000001"", ""Name"": ""Test Item 1"" "; 
        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "invalidTestData.json");
        File.WriteAllText(filePath, invalidJsonContent);

        // Act
        var act = () => JsonHelper.LoadDataFromJson<TestItem>(filePath);

        // Assert
        act.Should().Throw<Newtonsoft.Json.JsonSerializationException>();

        // Clean up
        File.Delete(filePath);
    }

    [Fact]
    public void LoadDataFromJson_ShouldThrowException_WhenFileDoesNotExist()
    {
        // Arrange
        var nonExistentFilePath = Path.Combine(Directory.GetCurrentDirectory(), "nonExistentFile.json");

        // Act
        var act = () => JsonHelper.LoadDataFromJson<TestItem>(nonExistentFilePath);

        // Assert
        act.Should().Throw<FileNotFoundException>();
    }

    private class TestItem
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = default!;
        public int Value { get; set; }
    }
}


