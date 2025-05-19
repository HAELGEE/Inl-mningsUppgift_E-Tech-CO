using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InlämningsUppgift_E_Tech_CO.Migrations
{
    /// <inheritdoc />
    public partial class update3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Top3",
                table: "Shop");

            migrationBuilder.AddColumn<bool>(
                name: "IsAtive",
                table: "Shop",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsAtive",
                table: "Shop");

            migrationBuilder.AddColumn<string>(
                name: "Top3",
                table: "Shop",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
