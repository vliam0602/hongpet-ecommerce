using HongPet.Application.AppConfigurations;

namespace HongPet.Application.Commons;
public class AppConfiguration
{
    public ConnectionStrings ConnectionStrings { get; set; } = default!;
    public JwtConfiguration JwtConfiguration { get; set; } = default!;
    public CorsPolicy CorsPolicy { get; set; } = default!;
}
