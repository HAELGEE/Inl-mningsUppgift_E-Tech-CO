using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InlämningsUppgift_E_Tech_CO.Migrations
{
    /// <inheritdoc />
    public partial class update4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExpressShipping",
                table: "Shop");

            migrationBuilder.DropColumn(
                name: "RegularShipping",
                table: "Shop");

            migrationBuilder.AddColumn<int>(
                name: "ExpressShipping",
                table: "Order",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RegularShipping",
                table: "Order",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExpressShipping",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "RegularShipping",
                table: "Order");

            migrationBuilder.AddColumn<int>(
                name: "ExpressShipping",
                table: "Shop",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RegularShipping",
                table: "Shop",
                type: "int",
                nullable: true);
        }
    }
}
