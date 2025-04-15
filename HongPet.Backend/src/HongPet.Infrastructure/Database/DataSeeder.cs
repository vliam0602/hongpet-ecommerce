using HongPet.Application.Heplers;
using HongPet.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace HongPet.Infrastructure.Database;

public class DataSeeder : IHostedService
{
    private readonly IServiceProvider _serviceProvider;

    public DataSeeder(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            if (!dbContext.Products.Any())
            {
                // seed products data
                var products = GetProductData();
                dbContext.Products.AddRange(products);
                await dbContext.SaveChangesAsync(cancellationToken);
            }
        }
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;

    private List<Product> GetProductData()
    {
        var filePath = Path.Combine(AppContext.BaseDirectory, "SeedingData", "products.json");
        var products = JsonHelper.LoadDataFromJson<Product>(filePath);
        var attributes = new Dictionary<Guid, ProductAttribute>();

        foreach (var product in products)
        {
            foreach (var variant in product.Variants)
            {
                foreach (var attrValue in variant.AttributeValues)
                {
                    var attrId = attrValue.Attribute.Id;
                    if (attributes.ContainsKey(attrId))
                    {
                        // Nếu đã có, gán lại để xài reference cũ
                        attrValue.Attribute = attributes[attrId];
                    } else
                    {
                        attributes[attrId] = attrValue.Attribute;
                    }
                }
            }
        }
        return products;        
    }
}

