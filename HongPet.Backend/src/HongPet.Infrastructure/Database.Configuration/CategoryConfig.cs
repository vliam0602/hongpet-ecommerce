using HongPet.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HongPet.Infrastructure.Database.Configuration;
public class CategoryConfig : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        // default value
        builder.Property(x => x.Id).HasDefaultValueSql("newid()");
        builder.Property(x => x.CreatedDate).HasDefaultValueSql("getutcdate()");
        builder.Property(x => x.LastModificatedDate).HasDefaultValueSql("getutcdate()");

        // add global query filter
        builder.HasQueryFilter(x => x.DeletedDate == null);

        // add index in name column
        builder.HasIndex(x => x.Name);

        #region init parent category data
        builder.HasData
        (
            new Category
            {
                Id = Guid.Parse("00000000-0000-0000-0000-000000000002"),
                Name = "Hamster",
                CreatedDate = DateTime.Parse("2025-04-15"),
                LastModificatedDate = DateTime.Parse("2025-04-15")
            },
            new Category
            {
                Id = Guid.Parse("00000000-0000-0000-0000-000000000003"),
                Name = "Chó",
                CreatedDate = DateTime.Parse("2025-04-15"),
                LastModificatedDate = DateTime.Parse("2025-04-15")
            },
            new Category
            {
                Id = Guid.Parse("00000000-0000-0000-0000-000000000004"),
                Name = "Mèo",
                CreatedDate = DateTime.Parse("2025-04-15"),
                LastModificatedDate = DateTime.Parse("2025-04-15")
            }
        );
        #endregion

        #region subcategory for Hamster
        builder.HasData
        (
            new Category
            {
                Id = Guid.Parse("00000000-0000-0000-0000-000000000005"),
                Name = "Thức ăn cho hamster",
                ParrentCategoryId = Guid.Parse("00000000-0000-0000-0000-000000000002"),
                CreatedDate = DateTime.Parse("2025-04-15"),
                LastModificatedDate = DateTime.Parse("2025-04-15")
            },
            new Category
            {
                Id = Guid.Parse("00000000-0000-0000-0000-000000000006"),
                Name = "Lồng - Chuồng cho hamster",
                ParrentCategoryId = Guid.Parse("00000000-0000-0000-0000-000000000002"),
                CreatedDate = DateTime.Parse("2025-04-15"),
                LastModificatedDate = DateTime.Parse("2025-04-15")
            },
            new Category
            {
                Id = Guid.Parse("00000000-0000-0000-0000-000000000007"),
                Name = "Đồ chơi - Phụ kiện cho hamster",
                ParrentCategoryId = Guid.Parse("00000000-0000-0000-0000-000000000002"),
                CreatedDate = DateTime.Parse("2025-04-15"),
                LastModificatedDate = DateTime.Parse("2025-04-15")
            },
            new Category
            {
                Id = Guid.Parse("00000000-0000-0000-0000-000000000008"),
                Name = "Vệ sinh cho hamster",
                ParrentCategoryId = Guid.Parse("00000000-0000-0000-0000-000000000002"),
                CreatedDate = DateTime.Parse("2025-04-15"),
                LastModificatedDate = DateTime.Parse("2025-04-15")
            },
            new Category
            {
                Id = Guid.Parse("00000000-0000-0000-0000-000000000009"),
                Name = "Chăm sóc sức khỏe cho hamster",
                ParrentCategoryId = Guid.Parse("00000000-0000-0000-0000-000000000002"),
                CreatedDate = DateTime.Parse("2025-04-15"),
                LastModificatedDate = DateTime.Parse("2025-04-15")
            }
        );
        #endregion

        #region subcategory for Chó
        builder.HasData
        (
            new Category
            {
                Id = Guid.Parse("00000000-0000-0000-0000-000000000010"),
                Name = "Thức ăn cho chó",
                ParrentCategoryId = Guid.Parse("00000000-0000-0000-0000-000000000003"),
                CreatedDate = DateTime.Parse("2025-04-15"),
                LastModificatedDate = DateTime.Parse("2025-04-15")
            },
            new Category
            {
                Id = Guid.Parse("00000000-0000-0000-0000-000000000011"),
                Name = "Bánh thưởng cho chó",
                ParrentCategoryId = Guid.Parse("00000000-0000-0000-0000-000000000003"),
                CreatedDate = DateTime.Parse("2025-04-15"),
                LastModificatedDate = DateTime.Parse("2025-04-15")
            },
            new Category
            {
                Id = Guid.Parse("00000000-0000-0000-0000-000000000012"),
                Name = "Đồ chơi - Phụ kiện cho chó",
                ParrentCategoryId = Guid.Parse("00000000-0000-0000-0000-000000000003"),
                CreatedDate = DateTime.Parse("2025-04-15"),
                LastModificatedDate = DateTime.Parse("2025-04-15")
            },
            new Category
            {
                Id = Guid.Parse("00000000-0000-0000-0000-000000000013"),
                Name = "Vệ sinh cho chó",
                ParrentCategoryId = Guid.Parse("00000000-0000-0000-0000-000000000003"),
                CreatedDate = DateTime.Parse("2025-04-15"),
                LastModificatedDate = DateTime.Parse("2025-04-15")
            },
            new Category
            {
                Id = Guid.Parse("00000000-0000-0000-0000-000000000014"),
                Name = "Chăm sóc sức khỏe cho chó",
                ParrentCategoryId = Guid.Parse("00000000-0000-0000-0000-000000000003"),
                CreatedDate = DateTime.Parse("2025-04-15"),
                LastModificatedDate = DateTime.Parse("2025-04-15")
            }
        );
        #endregion

        #region subcategory for Mèo
        builder.HasData
        (
            new Category
            {
                Id = Guid.Parse("00000000-0000-0000-0000-000000000015"),
                Name = "Thức ăn cho mèo",
                ParrentCategoryId = Guid.Parse("00000000-0000-0000-0000-000000000004"),
                CreatedDate = DateTime.Parse("2025-04-15"),
                LastModificatedDate = DateTime.Parse("2025-04-15")
            },
            new Category
            {
                Id = Guid.Parse("00000000-0000-0000-0000-000000000016"),
                Name = "Bánh thưởng cho mèo",
                ParrentCategoryId = Guid.Parse("00000000-0000-0000-0000-000000000004"),
                CreatedDate = DateTime.Parse("2025-04-15"),
                LastModificatedDate = DateTime.Parse("2025-04-15")
            },
            new Category
            {
                Id = Guid.Parse("00000000-0000-0000-0000-000000000017"),
                Name = "Đồ chơi - Phụ kiện cho mèo",
                ParrentCategoryId = Guid.Parse("00000000-0000-0000-0000-000000000004"),
                CreatedDate = DateTime.Parse("2025-04-15"),
                LastModificatedDate = DateTime.Parse("2025-04-15")
            },
            new Category
            {
                Id = Guid.Parse("00000000-0000-0000-0000-000000000018"),
                Name = "Vệ sinh cho mèo",
                ParrentCategoryId = Guid.Parse("00000000-0000-0000-0000-000000000004"),
                CreatedDate = DateTime.Parse("2025-04-15"),
                LastModificatedDate = DateTime.Parse("2025-04-15")
            },
            new Category
            {
                Id = Guid.Parse("00000000-0000-0000-0000-000000000019"),
                Name = "Chăm sóc sức khỏe cho mèo",
                ParrentCategoryId = Guid.Parse("00000000-0000-0000-0000-000000000004"),
                CreatedDate = DateTime.Parse("2025-04-15"),
                LastModificatedDate = DateTime.Parse("2025-04-15")
            }
        );
        #endregion
    }
}
