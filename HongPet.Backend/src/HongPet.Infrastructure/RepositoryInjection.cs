﻿using HongPet.Domain.Repositories.Abstractions;
using HongPet.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace HongPet.Infrastructure;
public static class RepositoryInjection
{
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IReviewRepository, ReviewRepository>();
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();

        return services;
    }
}
