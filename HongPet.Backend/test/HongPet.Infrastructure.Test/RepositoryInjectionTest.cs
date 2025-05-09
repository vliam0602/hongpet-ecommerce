using FluentAssertions;
using HongPet.Domain;
using HongPet.Domain.Repositories.Abstractions;
using HongPet.Infrastructure.Database;
using HongPet.WebApi;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace HongPet.Infrastructure.Test;

public class RepositoryInjectionTest
{
    [Fact]
    public void AddRepositories_ShouldRegisterAllRepositoriesCorrectly()
    {
        // Arrange
        var services = new ServiceCollection();

        services.AddDbContext<AppDbContext>(options =>
            options.UseInMemoryDatabase("TestRepoInjectionDB"));

        services.AddAutoMapper(typeof(MappingProfile), typeof(MappingDto));

        // Act
        services.AddRepositories();
        var serviceProvider = services.BuildServiceProvider();

        // Assert
        serviceProvider.GetService<IProductRepository>().Should().NotBeNull();
        serviceProvider.GetService<IUserRepository>().Should().NotBeNull();
        serviceProvider.GetService<IReviewRepository>().Should().NotBeNull();
        serviceProvider.GetService<IOrderRepository>().Should().NotBeNull();
        serviceProvider.GetService<ICategoryRepository>().Should().NotBeNull();
    }

    [Fact]
    public void AddRepositories_ShouldRegisterAllRepositoriesWithCorrectLifetime()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        services.AddRepositories();

        // Assert
        var serviceDescriptors = new[]
        {
            typeof(IProductRepository),
            typeof(IUserRepository),
            typeof(IReviewRepository),
            typeof(IOrderRepository),
            typeof(ICategoryRepository)
        };

        foreach (var serviceType in serviceDescriptors)
        {
            var descriptor = services.FirstOrDefault(d => d.ServiceType == serviceType);
            descriptor.Should().NotBeNull($"Repository {serviceType.Name} should be registered.");
            descriptor!.Lifetime.Should().Be(ServiceLifetime.Scoped, $"Repository {serviceType.Name} should have Scoped lifetime.");
        }
    }
}
