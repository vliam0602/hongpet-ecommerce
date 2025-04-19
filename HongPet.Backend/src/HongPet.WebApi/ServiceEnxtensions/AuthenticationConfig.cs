using HongPet.Application.Commons;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace HongPet.WebApi.ServiceEnxtensions;

public static class AuthenticationConfig
{
    public static IServiceCollection AddJwtConfiguration
        (this IServiceCollection services, AppConfiguration config)
    {
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(config.JwtConfiguration.SecretKey)),
                ClockSkew = TimeSpan.Zero // Remove delay - for test token exp =((
            };
        });
        return services;
    }
}
