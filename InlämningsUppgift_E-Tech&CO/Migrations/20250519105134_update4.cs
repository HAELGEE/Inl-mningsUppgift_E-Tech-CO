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
            migrationBuilder.RenameColumn(
                name: "IsAtive",
                table: "Shop",
                newName: "IsActive");

            migrationBuilder.AddColumn<int>(
                name: "IsActiveCategory",
                table: "Shop",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActiveCategory",
                table: "Shop");

            migrationBuilder.RenameColumn(
                name: "IsActive",
                table: "Shop",
                newName: "IsAtive");
        }
    }
}
