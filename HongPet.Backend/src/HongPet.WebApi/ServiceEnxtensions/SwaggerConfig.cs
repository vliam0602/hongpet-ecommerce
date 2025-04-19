using Microsoft.OpenApi.Models;

namespace HongPet.WebApi.ServiceEnxtensions;

public static class SwaggerConfig
{
    public static IServiceCollection AddSwaggerConfig(this IServiceCollection services)
    {
        services.AddRouting(options => options.LowercaseUrls = true);

        services.AddSwaggerGen(opt =>
        {
            opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Enter only token string (do not need 'Bearer')",
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                BearerFormat = "JWT",
                Scheme = "bearer"
            });

            opt.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type=ReferenceType.SecurityScheme,
                        Id="Bearer"
                    }
                },
                new string[]{}
                }
            });
        });
        return services;
    }
}
