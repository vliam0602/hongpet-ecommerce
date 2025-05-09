using AutoMapper;
using FluentAssertions;
using HongPet.Application;
using HongPet.Application.Commons;
using HongPet.Application.Services.Abstractions;
using HongPet.Application.Services.Commons;
using HongPet.Domain.Repositories.Abstractions;
using HongPet.Infrastructure;
using HongPet.Infrastructure.Database;
using HongPet.Infrastructure.Repositories.Commons;
using HongPet.WebApi;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace HongPet.Domain.Test.Application;

public class ServiceInjectionTest : SetupTest
{
    [Fact]
    public void AddApplicationServices_ShouldRegisterAllServicesCorrectly()
    {
        // Arrange        
        var services = new ServiceCollection();

        services.AddDbContext<AppDbContext>(options =>
            options.UseInMemoryDatabase("TestServiceInjectionDB"));

        services.AddAutoMapper(typeof(MappingProfile), typeof(MappingDto));

        services.AddHttpContextAccessor();

        services.AddRepositories();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        // Act
        services.AddApplicationServices();
        var serviceProvider = services.BuildServiceProvider();

        // Assert
        // Kiểm tra các dịch vụ được đăng ký đúng cách
        serviceProvider.GetService<IUserService>().Should().NotBeNull();
        serviceProvider.GetService<IProductService>().Should().NotBeNull();
        serviceProvider.GetService<IOrderService>().Should().NotBeNull();
        serviceProvider.GetService<IReviewService>().Should().NotBeNull();
        serviceProvider.GetService<ICategoryService>().Should().NotBeNull();
        serviceProvider.GetService<IClaimService>().Should().NotBeNull();
    }

    [Fact]
    public void AddApplicationServices_ShouldRegisterAllServicesWithCorrectLifetime()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        services.AddApplicationServices();
        var serviceProvider = services.BuildServiceProvider();

        // Assert
        // Danh sách các service cần kiểm tra
        var serviceDescriptors = new[]
        {
                typeof(IUserService),
                typeof(IProductService),
                typeof(IOrderService),
                typeof(IReviewService),
                typeof(ICategoryService),
                typeof(IClaimService),
                typeof(IGenericService<>)
            };

        foreach (var serviceType in serviceDescriptors)
        {            
            var descriptor = services.FirstOrDefault(d => d.ServiceType == serviceType);
            descriptor.Should().NotBeNull($"Service {serviceType.Name} should be registered.");
            descriptor!.Lifetime.Should().Be(ServiceLifetime.Scoped, $"Service {serviceType.Name} should have Scoped lifetime.");
        }
    }
}



