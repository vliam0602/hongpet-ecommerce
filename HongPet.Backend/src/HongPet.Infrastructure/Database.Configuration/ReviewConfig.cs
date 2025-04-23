using HongPet.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HongPet.Infrastructure.Database.Configuration;
public class ReviewConfig : IEntityTypeConfiguration<Review>
{
    public void Configure(EntityTypeBuilder<Review> builder)
    {
        builder.HasOne(x => x.Customer).WithMany(x => x.Reviews)
            .HasForeignKey(x => x.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Order).WithMany(x => x.Reviews)
            .HasForeignKey(x => x.OrderId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Product).WithMany(x => x.Reviews)
            .HasForeignKey(x => x.ProductId)
            .OnDelete(DeleteBehavior.Cascade);

        // add global query filter
        builder.HasQueryFilter(x => x.DeletedDate == null);

        #region init review data
        builder.HasData(
            new Review
            {
                Id = Guid.Parse("00000000-0000-0000-0000-000000000001"),
                OrderId = Guid.Parse("00000000-0000-0000-0000-000000000001"),
                CustomerId = Guid.Parse("00000000-0000-0000-0000-000000000002"), // Liam
                ProductId = Guid.Parse("00000000-0000-0000-0000-000000000001"), // Product 0001
                Rating = 5,
                Title = "Great product!",
                Comment = "My hamster loves it!",
                CreatedDate = DateTime.Parse("04-22-2025"),
                LastModificatedDate = DateTime.Parse("04-22-2025")
            },
            new Review
            {
                Id = Guid.Parse("00000000-0000-0000-0000-000000000002"),
                OrderId = Guid.Parse("00000000-0000-0000-0000-000000000002"),
                CustomerId = Guid.Parse("00000000-0000-0000-0000-000000000002"), // Liam
                ProductId = Guid.Parse("00000000-0000-0000-0000-000000000001"), // Product 0001
                Rating = 4,
                Title = "Good quality",
                Comment = "The product is good but delivery was slow.",
                CreatedDate = DateTime.Parse("04-22-2025"),
                LastModificatedDate = DateTime.Parse("04-22-2025")
            },
            new Review
            {
                Id = Guid.Parse("00000000-0000-0000-0000-000000000003"),
                OrderId = Guid.Parse("00000000-0000-0000-0000-000000000003"),
                CustomerId = Guid.Parse("00000000-0000-0000-0000-000000000002"), // Liam
                ProductId = Guid.Parse("00000000-0000-0000-0000-000000000001"), // Product 0001
                Rating = 3,
                Title = "Average",
                Comment = "It's okay, but I expected more.",
                CreatedDate = DateTime.Parse("04-22-2025"),
                LastModificatedDate = DateTime.Parse("04-22-2025")
            },
            new Review
            {
                Id = Guid.Parse("00000000-0000-0000-0000-000000000004"),
                OrderId = Guid.Parse("00000000-0000-0000-0000-000000000001"),
                CustomerId = Guid.Parse("00000000-0000-0000-0000-000000000002"), // Liam
                ProductId = Guid.Parse("00000000-0000-0000-0000-000000000001"), // Product 0001
                Rating = 5,
                Title = "Highly recommend",
                Comment = "Excellent product for the price.",
                CreatedDate = DateTime.Parse("04-22-2025"),
                LastModificatedDate = DateTime.Parse("04-22-2025")
            },
            new Review
            {
                Id = Guid.Parse("00000000-0000-0000-0000-000000000005"),
                OrderId = Guid.Parse("00000000-0000-0000-0000-000000000002"),
                CustomerId = Guid.Parse("00000000-0000-0000-0000-000000000002"), // Liam
                ProductId = Guid.Parse("00000000-0000-0000-0000-000000000001"), // Product 0001
                Rating = 4,
                Title = "Good value",
                Comment = "Worth the money.",
                CreatedDate = DateTime.Parse("04-22-2025"),
                LastModificatedDate = DateTime.Parse("04-22-2025")
            }
        );
        #endregion
    }
}
