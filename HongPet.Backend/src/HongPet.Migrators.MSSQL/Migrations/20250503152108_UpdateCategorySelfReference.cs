using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HongPet.Migrators.MSSQL.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCategorySelfReference : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ParrentCategoryId",
                table: "Categories");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000005"),
                column: "ParentCategoryId",
                value: new Guid("00000000-0000-0000-0000-000000000002"));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000006"),
                column: "ParentCategoryId",
                value: new Guid("00000000-0000-0000-0000-000000000002"));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000007"),
                column: "ParentCategoryId",
                value: new Guid("00000000-0000-0000-0000-000000000002"));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000008"),
                column: "ParentCategoryId",
                value: new Guid("00000000-0000-0000-0000-000000000002"));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000009"),
                column: "ParentCategoryId",
                value: new Guid("00000000-0000-0000-0000-000000000002"));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000010"),
                column: "ParentCategoryId",
                value: new Guid("00000000-0000-0000-0000-000000000003"));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000011"),
                column: "ParentCategoryId",
                value: new Guid("00000000-0000-0000-0000-000000000003"));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000012"),
                column: "ParentCategoryId",
                value: new Guid("00000000-0000-0000-0000-000000000003"));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000013"),
                column: "ParentCategoryId",
                value: new Guid("00000000-0000-0000-0000-000000000003"));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000014"),
                column: "ParentCategoryId",
                value: new Guid("00000000-0000-0000-0000-000000000003"));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000015"),
                column: "ParentCategoryId",
                value: new Guid("00000000-0000-0000-0000-000000000004"));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000016"),
                column: "ParentCategoryId",
                value: new Guid("00000000-0000-0000-0000-000000000004"));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000017"),
                column: "ParentCategoryId",
                value: new Guid("00000000-0000-0000-0000-000000000004"));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000018"),
                column: "ParentCategoryId",
                value: new Guid("00000000-0000-0000-0000-000000000004"));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000019"),
                column: "ParentCategoryId",
                value: new Guid("00000000-0000-0000-0000-000000000004"));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ParrentCategoryId",
                table: "Categories",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000002"),
                column: "ParrentCategoryId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000003"),
                column: "ParrentCategoryId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000004"),
                column: "ParrentCategoryId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000005"),
                columns: new[] { "ParentCategoryId", "ParrentCategoryId" },
                values: new object[] { null, new Guid("00000000-0000-0000-0000-000000000002") });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000006"),
                columns: new[] { "ParentCategoryId", "ParrentCategoryId" },
                values: new object[] { null, new Guid("00000000-0000-0000-0000-000000000002") });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000007"),
                columns: new[] { "ParentCategoryId", "ParrentCategoryId" },
                values: new object[] { null, new Guid("00000000-0000-0000-0000-000000000002") });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000008"),
                columns: new[] { "ParentCategoryId", "ParrentCategoryId" },
                values: new object[] { null, new Guid("00000000-0000-0000-0000-000000000002") });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000009"),
                columns: new[] { "ParentCategoryId", "ParrentCategoryId" },
                values: new object[] { null, new Guid("00000000-0000-0000-0000-000000000002") });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000010"),
                columns: new[] { "ParentCategoryId", "ParrentCategoryId" },
                values: new object[] { null, new Guid("00000000-0000-0000-0000-000000000003") });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000011"),
                columns: new[] { "ParentCategoryId", "ParrentCategoryId" },
                values: new object[] { null, new Guid("00000000-0000-0000-0000-000000000003") });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000012"),
                columns: new[] { "ParentCategoryId", "ParrentCategoryId" },
                values: new object[] { null, new Guid("00000000-0000-0000-0000-000000000003") });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000013"),
                columns: new[] { "ParentCategoryId", "ParrentCategoryId" },
                values: new object[] { null, new Guid("00000000-0000-0000-0000-000000000003") });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000014"),
                columns: new[] { "ParentCategoryId", "ParrentCategoryId" },
                values: new object[] { null, new Guid("00000000-0000-0000-0000-000000000003") });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000015"),
                columns: new[] { "ParentCategoryId", "ParrentCategoryId" },
                values: new object[] { null, new Guid("00000000-0000-0000-0000-000000000004") });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000016"),
                columns: new[] { "ParentCategoryId", "ParrentCategoryId" },
                values: new object[] { null, new Guid("00000000-0000-0000-0000-000000000004") });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000017"),
                columns: new[] { "ParentCategoryId", "ParrentCategoryId" },
                values: new object[] { null, new Guid("00000000-0000-0000-0000-000000000004") });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000018"),
                columns: new[] { "ParentCategoryId", "ParrentCategoryId" },
                values: new object[] { null, new Guid("00000000-0000-0000-0000-000000000004") });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000019"),
                columns: new[] { "ParentCategoryId", "ParrentCategoryId" },
                values: new object[] { null, new Guid("00000000-0000-0000-0000-000000000004") });
        }
    }
}
