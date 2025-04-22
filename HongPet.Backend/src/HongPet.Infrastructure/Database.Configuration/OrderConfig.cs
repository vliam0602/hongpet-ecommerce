using HongPet.Domain.Entities;
using HongPet.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HongPet.Infrastructure.Database.Configuration;
public class OrderConfig : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        // default value
        builder.Property(x => x.Id).HasDefaultValueSql("newid()");
        builder.Property(x => x.CreatedDate).HasDefaultValueSql("getutcdate()");
        builder.Property(x => x.LastModificatedDate).HasDefaultValueSql("getutcdate()");

        //#region Init Data
        //builder.HasData(
        //    new Order
        //    {
        //        Id = Guid.Parse("00000000-0000-0000-0000-000000000001"),
        //        CustomerId = Guid.Parse("00000000-0000-0000-0000-000000000002"), // Liam
        //        CustomerName = "Lam Lam",
        //        CustomerEmail = "liam@example.com",
        //        CustomerPhone = "0123456789",
        //        ShippingAddress = "123 Main Street",
        //        TotalAmount = 100000,
        //        Status = OrderStatusEnum.Completed,
        //        CreatedDate = DateTime.UtcNow,
        //        LastModificatedDate = DateTime.UtcNow,
        //        OrderItems = new List<OrderItem>
        //        {
        //            new OrderItem
        //            {
        //                OrderId = Guid.Parse("00000000-0000-0000-0000-000000000001"),
        //                VariantId = Guid.Parse("00000000-0000-0000-0000-000000000001"), // Variant of Product 0001
        //                Quantity = 2,
        //                Price = 50000
        //            }
        //        }
        //    },
        //    new Order
        //    {
        //        Id = Guid.Parse("00000000-0000-0000-0000-000000000002"),
        //        CustomerId = Guid.Parse("00000000-0000-0000-0000-000000000002"), // Liam
        //        CustomerName = "Lam Lam",
        //        CustomerEmail = "liam@example.com",
        //        CustomerPhone = "0123456789",
        //        ShippingAddress = "456 Elm Street",
        //        TotalAmount = 90000,
        //        Status = OrderStatusEnum.Completed,
        //        CreatedDate = DateTime.UtcNow,
        //        LastModificatedDate = DateTime.UtcNow,
        //        OrderItems = new List<OrderItem>
        //        {
        //            new OrderItem
        //            {
        //                OrderId = Guid.Parse("00000000-0000-0000-0000-000000000002"),
        //                VariantId = Guid.Parse("00000000-0000-0000-0000-000000000002"), // Variant of Product 0001
        //                Quantity = 1,
        //                Price = 90000
        //            }
        //        }
        //    },
        //    new Order
        //    {
        //        Id = Guid.Parse("00000000-0000-0000-0000-000000000003"),
        //        CustomerId = Guid.Parse("00000000-0000-0000-0000-000000000002"), // Liam
        //        CustomerName = "Lam Lam",
        //        CustomerEmail = "liam@example.com",
        //        CustomerPhone = "0123456789",
        //        ShippingAddress = "789 Oak Street",
        //        TotalAmount = 150000,
        //        Status = OrderStatusEnum.Completed,
        //        CreatedDate = DateTime.UtcNow,
        //        LastModificatedDate = DateTime.UtcNow,
        //        OrderItems = new List<OrderItem>
        //        {
        //            new OrderItem
        //            {
        //                OrderId = Guid.Parse("00000000-0000-0000-0000-000000000001"),
        //                VariantId = Guid.Parse("00000000-0000-0000-0000-000000000003"), // Variant of Product 0001
        //                Quantity = 3,
        //                Price = 50000
        //            }
        //        }
        //    }
        //);
        //#endregion
    }
}
