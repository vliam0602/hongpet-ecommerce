using HongPet.Application.Commons;
using HongPet.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();

// Bind AppConfiguration from configuration
var _config = builder.Configuration.Get<AppConfiguration>();
builder.Configuration.Bind(_config);
builder.Services.AddSingleton(_config!);

// Add dbcontext
builder.Services.AddDbContext<AppDbContext>(opt =>
            opt.UseSqlServer(_config!.ConnectionStrings.MSSQLServerDb,
                        x => x.MigrationsAssembly("HongPet.Migrators.MSSQL")));
// Seeidng data
builder.Services.AddHostedService<DataSeeder>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
