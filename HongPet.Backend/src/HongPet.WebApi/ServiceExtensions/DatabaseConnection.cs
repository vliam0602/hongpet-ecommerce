using HongPet.Application.Commons;
using HongPet.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace HongPet.WebApi.ServiceExtensions;

public static class DatabaseConnection
{
    public static IServiceCollection AddDbContextConfig(this IServiceCollection services, AppConfiguration config)
    {
        services.AddDbContext<AppDbContext>(opt =>
             opt.UseSqlServer
             (
                 connectionString: config!.ConnectionStrings.MSSQLServerDb,
                 sqlServerOptionsAction: x => x.MigrationsAssembly("HongPet.Migrators.MSSQL")
             ));
        return services;
    }
}
