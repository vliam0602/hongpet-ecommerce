using HongPet.Application;
using HongPet.Application.Commons;
using HongPet.Application.Services.Commons;
using HongPet.Domain.Repositories.Abstractions.Commons;
using HongPet.Infrastructure;
using HongPet.Infrastructure.Database;
using HongPet.Infrastructure.Repositories.Commons;
using HongPet.WebApi;
using HongPet.WebApi.ServiceEnxtensions;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddOpenApi();
builder.Services.AddSwaggerConfig();

// Bind AppConfiguration from configuration
var _config = builder.Configuration.Get<AppConfiguration>();
builder.Configuration.Bind(_config);
builder.Services.AddSingleton(_config!);

// Add dbcontext
builder.Services.AddDbContext<AppDbContext>(opt =>
            opt.UseSqlServer(_config!.ConnectionStrings.MSSQLServerDb,
                        x => x.MigrationsAssembly("HongPet.Migrators.MSSQL")));
// Seeding data
builder.Services.AddHostedService<DataSeeder>();

// Add jwt authentication
builder.Services.AddJwtConfiguration(_config!);

// Add unit of work (lazy load repositories inside)
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Add application services & repositories
builder.Services.AddRepositories();
builder.Services.AddApplicationServices();

// IHttpContextAccessor (for claim service)
builder.Services.AddHttpContextAccessor();

// Add auto mapper
builder.Services.AddAutoMapper(typeof(MappingProfile));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
