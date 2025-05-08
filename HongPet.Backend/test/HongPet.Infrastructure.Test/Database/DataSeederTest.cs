using FluentAssertions;
using HongPet.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace HongPet.Infrastructure.Test.Database
{
    public class DataSeederTest
    {
        private readonly Mock<IServiceProvider> _serviceProviderMock;
        private readonly Mock<IServiceScope> _serviceScopeMock;
        private readonly Mock<IServiceScopeFactory> _serviceScopeFactoryMock;
        private readonly AppDbContext _dbContext;

        public DataSeederTest()
        {
            // Set up InMemory database
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDataSeederDB")
                .Options;

            _dbContext = new AppDbContext(options);

            // Mock IServiceProvider and IServiceScope
            _serviceProviderMock = new Mock<IServiceProvider>();
            _serviceScopeMock = new Mock<IServiceScope>();
            _serviceScopeFactoryMock = new Mock<IServiceScopeFactory>();

            _serviceScopeMock
                .Setup(scope => scope.ServiceProvider)
                .Returns(_serviceProviderMock.Object);

            _serviceProviderMock
                .Setup(provider => provider.GetService(typeof(IServiceScopeFactory)))
                .Returns(_serviceScopeFactoryMock.Object);

            _serviceScopeFactoryMock
                .Setup(factory => factory.CreateScope())
                .Returns(_serviceScopeMock.Object);

            _serviceProviderMock
                .Setup(provider => provider.GetService(typeof(AppDbContext)))
                .Returns(_dbContext);
        }

        [Fact]
        public async Task DataSeeder_ShouldSeedAllData_WhenDatabaseIsEmpty()
        {
            // Arrange
            var dataSeeder = new DataSeeder(_serviceProviderMock.Object);

            // Create temporary JSON files for testing
            var basePath = Path.Combine(Directory.GetCurrentDirectory(), "SeedingData");
            Directory.CreateDirectory(basePath);

            var productFilePath = Path.Combine(basePath, "productData.json");
            var orderFilePath = Path.Combine(basePath, "orderData.json");
            var reviewFilePath = Path.Combine(basePath, "reviewData.json");
            var imageFilePath = Path.Combine(basePath, "imageData.json");

            await File.WriteAllTextAsync(productFilePath, GetProductDataJson());
            await File.WriteAllTextAsync(orderFilePath, GetOrderDataJson());
            await File.WriteAllTextAsync(reviewFilePath, GetReviewDataJson());
            await File.WriteAllTextAsync(imageFilePath, GetImageDataJson());

            // Act
            await dataSeeder.StartAsync(CancellationToken.None);

            // Assert
            _dbContext.Products.Should().NotBeEmpty();
            _dbContext.Orders.Should().NotBeEmpty();
            _dbContext.Reviews.Should().NotBeEmpty();
            _dbContext.Images.Should().NotBeEmpty();

            // Clean up temporary files
            Directory.Delete(basePath, true);
        }

        private string GetProductDataJson()
        {
            return @"
            [
                {
                    ""id"": ""00000000-0000-0000-0000-000000000001"",
                    ""name"": ""Thức ăn Hamster Classic"",
                    ""description"": ""Thức ăn dinh dưỡng dành cho hamster, dễ tiêu hóa."",
                    ""price"": 50000,
                    ""thumbnailUrl"": ""image-url"",
                    ""Categories"": [
                        { ""name"": ""Mèo"" },
                        { ""name"": ""Thức ăn cho mèo"" }
                    ],
                    ""variants"": [
                        {
                            ""id"": ""00000000-0000-0000-0000-000000000001"",
                            ""price"": 50000,
                            ""stock"": 100,
                            ""isActive"": true,
                            ""attributeValues"": [
                                {
                                    ""id"": ""00000000-0000-0000-0000-000000000001"",
                                    ""value"": ""100g"",
                                    ""attribute"": {
                                        ""id"": ""00000000-0000-0000-0000-000000000001"",
                                        ""name"": ""Khối lượng""
                                    }
                                }
                            ]
                        }
                    ]
                }
            ]";
        }

        private string GetOrderDataJson()
        {
            return @"
            [
                {
                    ""Id"": ""00000000-0000-0000-0000-000000000001"",
                    ""CustomerId"": ""00000000-0000-0000-0000-000000000002"",
                    ""CustomerName"": ""Lam Lam"",
                    ""CustomerEmail"": ""liam@example.com"",
                    ""CustomerPhone"": ""0123456789"",
                    ""ShippingAddress"": ""123 Main Street"",
                    ""TotalAmount"": 100000,
                    ""Status"": ""Completed"",
                    ""OrderItems"": [
                        {
                            ""OrderId"": ""00000000-0000-0000-0000-000000000001"",
                            ""VariantId"": ""00000000-0000-0000-0000-000000000001"",
                            ""Quantity"": 2,
                            ""Price"": 50000
                        }
                    ]
                }
            ]";
        }

        private string GetReviewDataJson()
        {
            return @"
            [
                {
                    ""Id"": ""00000000-0000-0000-0000-000000000001"",
                    ""OrderId"": ""00000000-0000-0000-0000-000000000001"",
                    ""CustomerId"": ""00000000-0000-0000-0000-000000000002"",
                    ""ProductId"": ""00000000-0000-0000-0000-000000000001"",
                    ""Rating"": 5,
                    ""Title"": ""Great product!"",
                    ""Comment"": ""My hamster loves it!""
                }
            ]";
        }

        private string GetImageDataJson()
        {
            return @"
            [
                {
                    ""Id"": ""00000000-0000-0000-0000-000000000001"",
                    ""ProductId"": ""00000000-0000-0000-0000-000000000001"",
                    ""Name"": ""Product 1 - Image 1"",
                    ""ImageUrl"": ""image-url""
                }
            ]";
        }
    }
}
