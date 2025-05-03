using HongPet.Application;
using HongPet.Application.Commons;
using HongPet.Domain;
using HongPet.Domain.Repositories.Abstractions;
using HongPet.Infrastructure;
using HongPet.Infrastructure.Database;
using HongPet.Infrastructure.Repositories.Commons;
using HongPet.WebApi;
using HongPet.WebApi.ServiceEnxtensions;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().AddJsonOptions(opt =>
{
    opt.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});
;

builder.Services.AddOpenApi();
builder.Services.AddSwaggerConfig();

// Bind AppConfiguration from configuration
var _config = builder.Configuration.Get<AppConfiguration>();
builder.Configuration.Bind(_config);
builder.Services.AddSingleton(_config!);

// Add Cors policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigins", policy =>
    {
        policy.WithOrigins(
                    _config!.CorsPolicy.AllowedOrigins.CustomerSite,
                    _config!.CorsPolicy.AllowedOrigins.AdminSite)
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});
// Add dbcontext
builder.Services.AddDbConfig(_config!);

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
builder.Services.AddAutoMapper(typeof(MappingDto));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowSpecificOrigins");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
