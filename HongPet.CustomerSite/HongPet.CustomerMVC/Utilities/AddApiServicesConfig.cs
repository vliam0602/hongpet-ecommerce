﻿using HongPet.CustomerMVC.Services;
using HongPet.CustomerMVC.Services.Abstraction;

namespace HongPet.CustomerMVC.Utilities;

public static class AddApiServicesConfig
{
    public static IServiceCollection AddApiServices(this IServiceCollection services, string baseUrl)
    {
        services.AddScoped<IClaimService, ClaimService>();
        
        services.AddScoped<ICartService, CartService>();

        services.AddHttpClient<IAuthApiService, AuthApiService>(client =>
        {
            client.BaseAddress = new Uri(baseUrl);
        });
        services.AddHttpClient<IProductApiService, ProductApiService>(client =>
        {
            client.BaseAddress = new Uri(baseUrl);
        });
        services.AddHttpClient<IOrderApiService, OrderApiService>(client =>
        {
            client.BaseAddress = new Uri(baseUrl);
        });
        return services;
    }
}
