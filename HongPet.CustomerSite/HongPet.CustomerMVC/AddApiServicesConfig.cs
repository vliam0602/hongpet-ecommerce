using HongPet.CustomerMVC.Services;
using HongPet.CustomerMVC.Services.Abstraction;

namespace HongPet.CustomerMVC;

public static class AddApiServicesConfig
{
    public static IServiceCollection AddApiServices(this IServiceCollection services, string baseUrl)
    {
        services.AddScoped<IClaimService, ClaimService>();

        services.AddHttpClient<IAuthApiService, AuthApiService>(client =>
        {
            client.BaseAddress = new Uri(baseUrl);
        });
        services.AddHttpClient<IProductApiService, ProductApiService>(client =>
        {
            client.BaseAddress = new Uri(baseUrl);
        });
        return services;
    }
}
