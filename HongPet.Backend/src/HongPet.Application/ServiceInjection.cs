﻿using HongPet.Application.Commons;
using HongPet.Application.Services;
using HongPet.Application.Services.Abstractions;
using HongPet.Application.Services.Commons;
using Microsoft.Extensions.DependencyInjection;

namespace HongPet.Application;
public static class ServiceInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped(typeof(IGenericService<>), typeof(GenericService<>));
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IOrderService, OrderService>();
        services.AddScoped<IReviewService, ReviewService>();
        services.AddScoped<ICategoryService, CategoryService>();
        
        services.AddScoped<IClaimService, ClaimService>();
        return services;
    }
}
