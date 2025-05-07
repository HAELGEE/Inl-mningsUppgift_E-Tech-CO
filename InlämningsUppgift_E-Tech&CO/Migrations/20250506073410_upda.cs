using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InlämningsUppgift_E_Tech_CO.Migrations
{
    /// <inheritdoc />
    public partial class upda : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Order_Shop_ShopId",
                table: "Order");

            migrationBuilder.DropIndex(
                name: "IX_Order_ShopId",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "ShopId",
                table: "Order");

            migrationBuilder.AddColumn<int>(
                name: "OrderId",
                table: "Shop",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Shop_OrderId",
                table: "Shop",
                column: "OrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_Shop_Order_OrderId",
                table: "Shop",
                column: "OrderId",
                principalTable: "Order",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Shop_Order_OrderId",
                table: "Shop");

            migrationBuilder.DropIndex(
                name: "IX_Shop_OrderId",
                table: "Shop");

            migrationBuilder.DropColumn(
                name: "OrderId",
                table: "Shop");

            migrationBuilder.AddColumn<int>(
                name: "ShopId",
                table: "Order",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Order_ShopId",
                table: "Order",
                column: "ShopId");

            migrationBuilder.AddForeignKey(
                name: "FK_Order_Shop_ShopId",
                table: "Order",
                column: "ShopId",
                principalTable: "Shop",
                principalColumn: "Id");
        }
    }
}
