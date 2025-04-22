using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HongPet.Migrators.MSSQL.Migrations
{
    /// <inheritdoc />
    public partial class UpdateProductAndReviewSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Reviews",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Products",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Title",
                table: "Reviews");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Products");
        }
    }
}
