namespace HongPet.Application.AppConfigurations;
public class CorsPolicy
{
    public AllowedOrigins AllowedOrigins { get; set; } = default!;
}

public class AllowedOrigins
{
    public string CustomerSite { get; set; } = default!;
    public string AdminSite { get; set; } = default!;
}
