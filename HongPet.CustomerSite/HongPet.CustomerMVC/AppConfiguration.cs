namespace HongPet.CustomerMVC;

public class AppConfiguration
{
    public ApiSettings ApiSettings { get; set; } = default!;
}

public class ApiSettings
{
    public string BaseUrl { get; set; } = default!;
}
