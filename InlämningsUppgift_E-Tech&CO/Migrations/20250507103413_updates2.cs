using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InlämningsUppgift_E_Tech_CO.Migrations
{
    /// <inheritdoc />
    public partial class updates2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TotalAmount",
                table: "Order",
                newName: "TotalItems");

            migrationBuilder.AddColumn<int>(
                name: "TotalAmountPrice",
                table: "Order",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalAmountPrice",
                table: "Order");

            migrationBuilder.RenameColumn(
                name: "TotalItems",
                table: "Order",
                newName: "TotalAmount");
        }
    }
}
