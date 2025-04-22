using HongPet.Application.Heplers;
using HongPet.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
                // reset the attr, and attrValue data to seed product data
                dbContext.ProductAttributeValues.RemoveRange(dbContext.ProductAttributeValues);
                dbContext.ProductAttributes.RemoveRange(dbContext.ProductAttributes);
                
                // seed products data
                var products = GetProductData();

                // handle the category os products
                foreach (var product in products)
                {
                    var categories = product.Categories.ToList();
                    for (int i = 0; i < categories.Count; i++)
                    {
                        var category = categories[i];
                        var existedCategory = await dbContext.Categories
                            .FirstOrDefaultAsync(c => c.Name == category.Name,
                                                cancellationToken);
                        if (existedCategory != null)
                        {
                            categories[i] = existedCategory;
                        }
                    }
                }
                // seed products data
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
        var attributeDict = new Dictionary<Guid, ProductAttribute>();
        var attributeValueDict = new Dictionary<Guid, ProductAttributeValue>();

        foreach (var product in products)
        {
            foreach (var variant in product.Variants)
            {
                var attributeValuesList = variant.AttributeValues.ToList(); // Convert IEnumerable to List  

                for (int i = 0; i < attributeValuesList.Count; i++)
                {
                    var attrValue = attributeValuesList[i];

                    // Handle Attribute  
                    var attrId = attrValue.Attribute.Id;
                    if (attributeDict.TryGetValue(attrId, out var existingAttribute))
                    {
                        // if existed, use the old obj to reuse the reference
                        attrValue.Attribute = existingAttribute;
                    } else
                    {
                        // if not, add new
                        attributeDict[attrId] = attrValue.Attribute;
                    }

                    // Handle AttributeValue  
                    var attrValueId = attrValue.Id;
                    if (attributeValueDict.TryGetValue(attrValueId, out var existingAttrValue))
                    {
                        // if existed, use the old obj to reuse the reference
                        attributeValuesList[i] = existingAttrValue;
                    } else
                    {
                        // if not, add new
                        attributeValueDict[attrValueId] = attrValue;
                    }
                }

                // Update the original IEnumerable with the modified list  
                variant.AttributeValues = attributeValuesList;
            }
        }

        return products;
    }

}

