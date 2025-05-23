﻿using HongPet.Domain.Entities;
using HongPet.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HongPet.Infrastructure.Database.Configuration;
public class UserConfig : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasIndex(x => x.Email).IsUnique();

        // default value
        builder.Property(x => x.Id).HasDefaultValueSql("newid()");
        builder.Property(x => x.CreatedDate).HasDefaultValueSql("getutcdate()");
        builder.Property(x => x.LastModificatedDate).HasDefaultValueSql("getutcdate()");

        // add global query filter
        //builder.HasQueryFilter(x => x.DeletedDate == null);

        #region init data
        builder.HasData
        (
            // admin
            new User
            {
                Id = Guid.Parse("00000000-0000-0000-0000-000000000001"),
                Email = "admin@example.com",
                Username = "admin",
                Password = "t3sQhtkqtj41Row1AsEIUURPf5NAt7dh+gIKNLpMhxmZ9sHs", // hash password of "P@ss123"
                Fullname = "Admin",
                Role = RoleEnum.Admin,
                AvatarUrl = "https://i.pinimg.com/736x/34/60/3c/34603ce8a80b1ce9a768cad7ebf63c56.jpg",
                CreatedDate = DateTime.Parse("2025-04-15"),
                LastModificatedDate = DateTime.Parse("2025-04-15")
            },
            // customer 1
            new User
            {
                Id = Guid.Parse("00000000-0000-0000-0000-000000000002"),
                Email = "liam@example.com",
                Username = "liam",
                Password = "t3sQhtkqtj41Row1AsEIUURPf5NAt7dh+gIKNLpMhxmZ9sHs", // hash password of "P@ss123"
                Fullname = "Lam Lam",
                AvatarUrl = "https://cdn11.dienmaycholon.vn/filewebdmclnew/public/userupload/files/Image%20FP_2024/avatar-cute-54.png",
                Role = RoleEnum.Customer,
                CreatedDate = DateTime.Parse("2025-04-15"),
                LastModificatedDate = DateTime.Parse("2025-04-15")
            }
        );
        #endregion
    }
}
