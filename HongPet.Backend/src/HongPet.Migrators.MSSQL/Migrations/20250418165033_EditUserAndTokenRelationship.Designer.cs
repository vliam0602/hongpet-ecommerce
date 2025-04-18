﻿// <auto-generated />
using System;
using HongPet.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace HongPet.Migrators.MSSQL.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20250418165033_EditUserAndTokenRelationship")]
    partial class EditUserAndTokenRelationship
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("CategoryProduct", b =>
                {
                    b.Property<Guid>("CategoriesId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ProductsId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("CategoriesId", "ProductsId");

                    b.HasIndex("ProductsId");

                    b.ToTable("CategoryProduct");
                });

            modelBuilder.Entity("HongPet.Domain.Entities.Category", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasDefaultValueSql("newid()");

                    b.Property<Guid?>("CreatedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("getutcdate()");

                    b.Property<Guid?>("DeletedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("DeletedDate")
                        .HasColumnType("datetime2");

                    b.Property<Guid?>("LastModificatedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("LastModificatedDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("getutcdate()");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("ParentCategoryId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("ParrentCategoryId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("ParentCategoryId");

                    b.ToTable("Categories");

                    b.HasData(
                        new
                        {
                            Id = new Guid("00000000-0000-0000-0000-000000000002"),
                            CreatedDate = new DateTime(2025, 4, 15, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            LastModificatedDate = new DateTime(2025, 4, 15, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Hamster"
                        },
                        new
                        {
                            Id = new Guid("00000000-0000-0000-0000-000000000003"),
                            CreatedDate = new DateTime(2025, 4, 15, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            LastModificatedDate = new DateTime(2025, 4, 15, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Chó"
                        },
                        new
                        {
                            Id = new Guid("00000000-0000-0000-0000-000000000004"),
                            CreatedDate = new DateTime(2025, 4, 15, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            LastModificatedDate = new DateTime(2025, 4, 15, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Mèo"
                        },
                        new
                        {
                            Id = new Guid("00000000-0000-0000-0000-000000000005"),
                            CreatedDate = new DateTime(2025, 4, 15, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            LastModificatedDate = new DateTime(2025, 4, 15, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Thức ăn",
                            ParrentCategoryId = new Guid("00000000-0000-0000-0000-000000000002")
                        },
                        new
                        {
                            Id = new Guid("00000000-0000-0000-0000-000000000006"),
                            CreatedDate = new DateTime(2025, 4, 15, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            LastModificatedDate = new DateTime(2025, 4, 15, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Lồng - Chuồng",
                            ParrentCategoryId = new Guid("00000000-0000-0000-0000-000000000002")
                        },
                        new
                        {
                            Id = new Guid("00000000-0000-0000-0000-000000000007"),
                            CreatedDate = new DateTime(2025, 4, 15, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            LastModificatedDate = new DateTime(2025, 4, 15, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Đồ chơi - Phụ kiện",
                            ParrentCategoryId = new Guid("00000000-0000-0000-0000-000000000002")
                        },
                        new
                        {
                            Id = new Guid("00000000-0000-0000-0000-000000000008"),
                            CreatedDate = new DateTime(2025, 4, 15, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            LastModificatedDate = new DateTime(2025, 4, 15, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Vệ sinh",
                            ParrentCategoryId = new Guid("00000000-0000-0000-0000-000000000002")
                        },
                        new
                        {
                            Id = new Guid("00000000-0000-0000-0000-000000000009"),
                            CreatedDate = new DateTime(2025, 4, 15, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            LastModificatedDate = new DateTime(2025, 4, 15, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Chăm sóc sức khỏe",
                            ParrentCategoryId = new Guid("00000000-0000-0000-0000-000000000002")
                        },
                        new
                        {
                            Id = new Guid("00000000-0000-0000-0000-000000000010"),
                            CreatedDate = new DateTime(2025, 4, 15, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            LastModificatedDate = new DateTime(2025, 4, 15, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Thức ăn",
                            ParrentCategoryId = new Guid("00000000-0000-0000-0000-000000000003")
                        },
                        new
                        {
                            Id = new Guid("00000000-0000-0000-0000-000000000011"),
                            CreatedDate = new DateTime(2025, 4, 15, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            LastModificatedDate = new DateTime(2025, 4, 15, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Bánh thưởng",
                            ParrentCategoryId = new Guid("00000000-0000-0000-0000-000000000003")
                        },
                        new
                        {
                            Id = new Guid("00000000-0000-0000-0000-000000000012"),
                            CreatedDate = new DateTime(2025, 4, 15, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            LastModificatedDate = new DateTime(2025, 4, 15, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Đồ chơi - Phụ kiện",
                            ParrentCategoryId = new Guid("00000000-0000-0000-0000-000000000003")
                        },
                        new
                        {
                            Id = new Guid("00000000-0000-0000-0000-000000000013"),
                            CreatedDate = new DateTime(2025, 4, 15, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            LastModificatedDate = new DateTime(2025, 4, 15, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Vệ sinh",
                            ParrentCategoryId = new Guid("00000000-0000-0000-0000-000000000003")
                        },
                        new
                        {
                            Id = new Guid("00000000-0000-0000-0000-000000000014"),
                            CreatedDate = new DateTime(2025, 4, 15, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            LastModificatedDate = new DateTime(2025, 4, 15, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Chăm sóc sức khỏe",
                            ParrentCategoryId = new Guid("00000000-0000-0000-0000-000000000003")
                        },
                        new
                        {
                            Id = new Guid("00000000-0000-0000-0000-000000000015"),
                            CreatedDate = new DateTime(2025, 4, 15, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            LastModificatedDate = new DateTime(2025, 4, 15, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Thức ăn",
                            ParrentCategoryId = new Guid("00000000-0000-0000-0000-000000000004")
                        },
                        new
                        {
                            Id = new Guid("00000000-0000-0000-0000-000000000016"),
                            CreatedDate = new DateTime(2025, 4, 15, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            LastModificatedDate = new DateTime(2025, 4, 15, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Bánh thưởng",
                            ParrentCategoryId = new Guid("00000000-0000-0000-0000-000000000004")
                        },
                        new
                        {
                            Id = new Guid("00000000-0000-0000-0000-000000000017"),
                            CreatedDate = new DateTime(2025, 4, 15, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            LastModificatedDate = new DateTime(2025, 4, 15, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Đồ chơi - Phụ kiện",
                            ParrentCategoryId = new Guid("00000000-0000-0000-0000-000000000004")
                        },
                        new
                        {
                            Id = new Guid("00000000-0000-0000-0000-000000000018"),
                            CreatedDate = new DateTime(2025, 4, 15, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            LastModificatedDate = new DateTime(2025, 4, 15, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Vệ sinh",
                            ParrentCategoryId = new Guid("00000000-0000-0000-0000-000000000004")
                        },
                        new
                        {
                            Id = new Guid("00000000-0000-0000-0000-000000000019"),
                            CreatedDate = new DateTime(2025, 4, 15, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            LastModificatedDate = new DateTime(2025, 4, 15, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Chăm sóc sức khỏe",
                            ParrentCategoryId = new Guid("00000000-0000-0000-0000-000000000004")
                        });
                });

            modelBuilder.Entity("HongPet.Domain.Entities.Image", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("CreatedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<Guid?>("DeletedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("DeletedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("ImageUrl")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("LastModificatedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("LastModificatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("ProductId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("ProductId");

                    b.ToTable("Images");
                });

            modelBuilder.Entity("HongPet.Domain.Entities.Order", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("CreatedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("CustomerEmail")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("CustomerId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("CustomerName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CustomerPhone")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("DeletedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("DeletedDate")
                        .HasColumnType("datetime2");

                    b.Property<Guid?>("LastModificatedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("LastModificatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("ShippingAddress")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<decimal>("TotalAmount")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Id");

                    b.HasIndex("CustomerId");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("HongPet.Domain.Entities.OrderItem", b =>
                {
                    b.Property<Guid>("OrderId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("VariantId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.HasKey("OrderId", "VariantId");

                    b.HasIndex("VariantId");

                    b.ToTable("OrderItems");
                });

            modelBuilder.Entity("HongPet.Domain.Entities.Product", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasDefaultValueSql("newid()");

                    b.Property<Guid?>("CreatedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("getutcdate()");

                    b.Property<Guid?>("DeletedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("DeletedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("LastModificatedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("LastModificatedDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("getutcdate()");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("HongPet.Domain.Entities.ProductAttribute", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("CreatedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<Guid?>("DeletedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("DeletedDate")
                        .HasColumnType("datetime2");

                    b.Property<Guid?>("LastModificatedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("LastModificatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("ProductAttributes");
                });

            modelBuilder.Entity("HongPet.Domain.Entities.ProductAttributeValue", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("AttributeId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("CreatedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<Guid?>("DeletedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("DeletedDate")
                        .HasColumnType("datetime2");

                    b.Property<Guid?>("LastModificatedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("LastModificatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("AttributeId");

                    b.ToTable("ProductAttributeValues");
                });

            modelBuilder.Entity("HongPet.Domain.Entities.Review", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Comment")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("CreatedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("CustomerId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("DeletedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("DeletedDate")
                        .HasColumnType("datetime2");

                    b.Property<Guid?>("LastModificatedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("LastModificatedDate")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("OrderId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ProductId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Rating")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CustomerId");

                    b.HasIndex("OrderId");

                    b.HasIndex("ProductId");

                    b.ToTable("Reviews");
                });

            modelBuilder.Entity("HongPet.Domain.Entities.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasDefaultValueSql("newid()");

                    b.Property<string>("Address")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("AvatarUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("CreatedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("getutcdate()");

                    b.Property<Guid?>("DeletedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("DeletedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Fullname")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("LastModificatedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("LastModificatedDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("getutcdate()");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Role")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            Id = new Guid("00000000-0000-0000-0000-000000000001"),
                            CreatedDate = new DateTime(2025, 4, 15, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Email = "admin@example.com",
                            Fullname = "Admin",
                            LastModificatedDate = new DateTime(2025, 4, 15, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Password = "t3sQhtkqtj41Row1AsEIUURPf5NAt7dh+gIKNLpMhxmZ9sHs",
                            Role = 2
                        },
                        new
                        {
                            Id = new Guid("00000000-0000-0000-0000-000000000002"),
                            CreatedDate = new DateTime(2025, 4, 15, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Email = "liam@example.com",
                            Fullname = "Lam Lam",
                            LastModificatedDate = new DateTime(2025, 4, 15, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Password = "t3sQhtkqtj41Row1AsEIUURPf5NAt7dh+gIKNLpMhxmZ9sHs",
                            Role = 1
                        });
                });

            modelBuilder.Entity("HongPet.Domain.Entities.UserToken", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ATid")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("CreatedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<Guid?>("DeletedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("DeletedDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("ExpiredDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("IssuedDate")
                        .HasColumnType("datetime2");

                    b.Property<Guid?>("LastModificatedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("LastModificatedDate")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("RTid")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("RefreshToken")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("UserTokens");
                });

            modelBuilder.Entity("HongPet.Domain.Entities.Variant", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("CreatedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<Guid?>("DeletedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("DeletedDate")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<Guid?>("LastModificatedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("LastModificatedDate")
                        .HasColumnType("datetime2");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18,2)");

                    b.Property<Guid>("ProductId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Stock")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ProductId");

                    b.ToTable("Variants");
                });

            modelBuilder.Entity("ProductAttributeValueVariant", b =>
                {
                    b.Property<Guid>("AttributeValuesId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("VariantsId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("AttributeValuesId", "VariantsId");

                    b.HasIndex("VariantsId");

                    b.ToTable("ProductAttributeValueVariant");
                });

            modelBuilder.Entity("CategoryProduct", b =>
                {
                    b.HasOne("HongPet.Domain.Entities.Category", null)
                        .WithMany()
                        .HasForeignKey("CategoriesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("HongPet.Domain.Entities.Product", null)
                        .WithMany()
                        .HasForeignKey("ProductsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("HongPet.Domain.Entities.Category", b =>
                {
                    b.HasOne("HongPet.Domain.Entities.Category", "ParentCategory")
                        .WithMany("SubCategories")
                        .HasForeignKey("ParentCategoryId");

                    b.Navigation("ParentCategory");
                });

            modelBuilder.Entity("HongPet.Domain.Entities.Image", b =>
                {
                    b.HasOne("HongPet.Domain.Entities.Product", "Product")
                        .WithMany("Images")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Product");
                });

            modelBuilder.Entity("HongPet.Domain.Entities.Order", b =>
                {
                    b.HasOne("HongPet.Domain.Entities.User", "Customer")
                        .WithMany("Orders")
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Customer");
                });

            modelBuilder.Entity("HongPet.Domain.Entities.OrderItem", b =>
                {
                    b.HasOne("HongPet.Domain.Entities.Order", "Order")
                        .WithMany("OrderItems")
                        .HasForeignKey("OrderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("HongPet.Domain.Entities.Variant", "Variant")
                        .WithMany("OrderItems")
                        .HasForeignKey("VariantId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Order");

                    b.Navigation("Variant");
                });

            modelBuilder.Entity("HongPet.Domain.Entities.ProductAttributeValue", b =>
                {
                    b.HasOne("HongPet.Domain.Entities.ProductAttribute", "Attribute")
                        .WithMany("AttributeValues")
                        .HasForeignKey("AttributeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Attribute");
                });

            modelBuilder.Entity("HongPet.Domain.Entities.Review", b =>
                {
                    b.HasOne("HongPet.Domain.Entities.User", "Customer")
                        .WithMany("Reviews")
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("HongPet.Domain.Entities.Order", "Order")
                        .WithMany("Reviews")
                        .HasForeignKey("OrderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("HongPet.Domain.Entities.Product", "Product")
                        .WithMany("Reviews")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Customer");

                    b.Navigation("Order");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("HongPet.Domain.Entities.UserToken", b =>
                {
                    b.HasOne("HongPet.Domain.Entities.User", "User")
                        .WithMany("UserToken")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("HongPet.Domain.Entities.Variant", b =>
                {
                    b.HasOne("HongPet.Domain.Entities.Product", "Product")
                        .WithMany("Variants")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Product");
                });

            modelBuilder.Entity("ProductAttributeValueVariant", b =>
                {
                    b.HasOne("HongPet.Domain.Entities.ProductAttributeValue", null)
                        .WithMany()
                        .HasForeignKey("AttributeValuesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("HongPet.Domain.Entities.Variant", null)
                        .WithMany()
                        .HasForeignKey("VariantsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("HongPet.Domain.Entities.Category", b =>
                {
                    b.Navigation("SubCategories");
                });

            modelBuilder.Entity("HongPet.Domain.Entities.Order", b =>
                {
                    b.Navigation("OrderItems");

                    b.Navigation("Reviews");
                });

            modelBuilder.Entity("HongPet.Domain.Entities.Product", b =>
                {
                    b.Navigation("Images");

                    b.Navigation("Reviews");

                    b.Navigation("Variants");
                });

            modelBuilder.Entity("HongPet.Domain.Entities.ProductAttribute", b =>
                {
                    b.Navigation("AttributeValues");
                });

            modelBuilder.Entity("HongPet.Domain.Entities.User", b =>
                {
                    b.Navigation("Orders");

                    b.Navigation("Reviews");

                    b.Navigation("UserToken");
                });

            modelBuilder.Entity("HongPet.Domain.Entities.Variant", b =>
                {
                    b.Navigation("OrderItems");
                });
#pragma warning restore 612, 618
        }
    }
}
