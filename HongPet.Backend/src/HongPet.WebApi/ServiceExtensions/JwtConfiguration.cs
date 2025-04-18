using HongPet.Application.Commons;
using HongPet.Application.Services.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json;

namespace HongPet.WebApi.ServiceExtensions;

public static class JwtConfiguration
{
    public static IServiceCollection AddJwtConfiguration(this IServiceCollection services, AppConfiguration config)
    {
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = config?.JwtConfiguration.Issuer,
                ValidAudience = config?.JwtConfiguration.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(config?.JwtConfiguration.SecretKey!))
            };

            // Xử lý sự kiện khi token được validate
            options.Events = new JwtBearerEvents
            {
                OnTokenValidated = async context =>
                {
                    // Gọi hàm ValidateAccessTokenAsync trong TokenHandler
                    var tokenHandler = context.HttpContext
                                              .RequestServices
                                              .GetRequiredService<ITokenHandler>();

                    await tokenHandler.ValidateAccessTokenAsync(context);
                },
                OnAuthenticationFailed = context =>
                {
                    context.Response.StatusCode = 401;
                    context.Response.ContentType = "application/json";
                    return context.Response.WriteAsync(JsonSerializer.Serialize(new
                    {
                        error = "Authentication failed",
                        details = context.Exception.Message
                    }));
                }
            };
        });
        return services;
    }
}
