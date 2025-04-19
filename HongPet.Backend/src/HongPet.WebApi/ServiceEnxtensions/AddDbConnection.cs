using HongPet.Application.Commons;
using HongPet.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace HongPet.WebApi.ServiceEnxtensions;

public static class AddDbConnection
{
    public static IServiceCollection AddDbConfig(this IServiceCollection services, AppConfiguration config)
    {
        services.AddDbContext<AppDbContext>(opt =>
            opt.UseSqlServer(config!.ConnectionStrings.MSSQLServerDb,
                        x => x.MigrationsAssembly("HongPet.Migrators.MSSQL")));
        return services;
    }
}
