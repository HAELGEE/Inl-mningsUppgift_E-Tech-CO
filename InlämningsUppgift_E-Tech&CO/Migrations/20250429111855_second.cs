using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InlämningsUppgift_E_Tech_CO.Migrations
{
    /// <inheritdoc />
    public partial class second : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "LoggedIn",
                table: "Customer",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "Customer",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "Customer",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LoggedIn",
                table: "Customer");

            migrationBuilder.DropColumn(
                name: "Password",
                table: "Customer");

            migrationBuilder.DropColumn(
                name: "UserName",
                table: "Customer");
        }
    }
}
