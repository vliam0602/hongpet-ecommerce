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
        var basePath = Path.Combine(AppContext.BaseDirectory, "SeedingData");
        var productFilePath = Path.Combine(basePath, "productData.json");
        var orderFilePath = Path.Combine(basePath, "orderData.json");
        var reviewFilePath = Path.Combine(basePath, "reviewData.json");
        var imageFilePath = Path.Combine(basePath, "imageData.json");

        using (var scope = _serviceProvider.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            await SeedProductDataAsync(productFilePath, dbContext, cancellationToken);

            await SeedOrderDataAsync(orderFilePath, dbContext, cancellationToken);

            await SeedReviewDataAsync(reviewFilePath, dbContext, cancellationToken);
            
            await SeedImageDataAsync(imageFilePath, dbContext, cancellationToken);

            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;

    private async Task SeedProductDataAsync(string filePath, 
        AppDbContext dbContext, CancellationToken cancellationToken)
    {
        if (!dbContext.Products.Any())
        {
            // reset the attr, and attrValue data to seed product data
            dbContext.ProductAttributeValues.RemoveRange(dbContext.ProductAttributeValues);
            dbContext.ProductAttributes.RemoveRange(dbContext.ProductAttributes);

            // seed products data
            var products = GetProductData(filePath);

            // handle the category os products
            foreach (var product in products)
            {
                var categories = product.Categories.ToList();
                for (int i = 0; i < categories.Count; i++)
                {
                    var category = categories[i];
                    var existedCategory = await dbContext.Categories
                        .FirstOrDefaultAsync(c => c.Name.Equals(category.Name),
                                            cancellationToken);
                    if (existedCategory != null)
                    {
                        categories[i] = existedCategory;
                    }
                }
                product.Categories = categories;
            }           
            await dbContext.Products.AddRangeAsync(products);
        }
    }

    private List<Product> GetProductData(string filePath)
    {
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


    private async Task SeedOrderDataAsync(string filePath, 
        AppDbContext dbContext, CancellationToken cancellationToken)
    {
        if (!dbContext.Orders.Any())
        {
            // reset the orderItem data to seed order data
            dbContext.OrderItems.RemoveRange(dbContext.OrderItems);

            // seed orders data
            var orders = JsonHelper.LoadDataFromJson<Order>(filePath);
            await dbContext.Orders.AddRangeAsync(orders);
        }
    }

    private async Task SeedReviewDataAsync(string filePath, 
        AppDbContext dbContext, CancellationToken cancellationToken)
    {
        if (!dbContext.Reviews.Any())
        {
            // seed reviews data
            var reviews = JsonHelper.LoadDataFromJson<Review>(filePath);
            await dbContext.Reviews.AddRangeAsync(reviews);
        }
    }

    private async Task SeedImageDataAsync(string filePath,
        AppDbContext dbContext, CancellationToken cancellationToken)
    {
        if (!dbContext.Images.Any())
        {
            // seed reviews data
            var images = JsonHelper.LoadDataFromJson<Image>(filePath);
            await dbContext.Images.AddRangeAsync(images);
        }
    }
}

