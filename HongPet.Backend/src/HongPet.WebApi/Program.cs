using HongPet.Application;
using HongPet.Application.Commons;
using HongPet.Domain.Repositories.Abstractions.Commons;
using HongPet.Infrastructure;
using HongPet.Infrastructure.Database;
using HongPet.Infrastructure.Repositories.Commons;
using HongPet.WebApi;
using HongPet.WebApi.BuilderExtensions;
using HongPet.WebApi.ServiceExtensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// Add swagger config
builder.Services.AddOpenApi();
builder.Services.AddSwagggerConfig();

builder.Services.AddRouting(options => options.LowercaseUrls = true);

// Bind AppConfiguration from configuration
var _config = builder.Configuration.Get<AppConfiguration>();
builder.Configuration.Bind(_config);
builder.Services.AddSingleton(_config!);

// Add dbcontext
builder.Services.AddDbContextConfig(_config!);

// Seeding data
builder.Services.AddHostedService<DataSeeder>();

// Add unit of work - lazy load repositories inside
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Add application services & repositories
builder.Services.AddApplicationServices();
builder.Services.AddRepositories();

// Add IHttpContextAccessor (for claim service)
builder.Services.AddHttpContextAccessor();

// Add auto mapper
builder.Services.AddAutoMapper(typeof(MappingProfile));

// Add jwt authentication & authorization
builder.Services.AddJwtConfiguration(_config!);

builder.Services.AddAuthorization();


var app = builder.Build();

app.MapOpenApi();
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();


app.Run();
