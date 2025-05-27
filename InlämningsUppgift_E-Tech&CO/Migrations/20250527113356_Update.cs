using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InlämningsUppgift_E_Tech_CO.Migrations
{
    /// <inheritdoc />
    public partial class Update : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Category",
                table: "Shop");

            migrationBuilder.DropColumn(
                name: "SubCategory",
                table: "Shop");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Category",
                table: "Shop",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SubCategory",
                table: "Shop",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
