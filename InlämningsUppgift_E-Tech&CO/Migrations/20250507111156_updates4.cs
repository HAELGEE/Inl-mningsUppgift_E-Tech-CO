using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InlämningsUppgift_E_Tech_CO.Migrations
{
    /// <inheritdoc />
    public partial class updates4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderShop_Order_OrdersId",
                table: "OrderShop");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderShop_Shop_ShopsId",
                table: "OrderShop");

            migrationBuilder.RenameColumn(
                name: "ShopsId",
                table: "OrderShop",
                newName: "ShopId");

            migrationBuilder.RenameColumn(
                name: "OrdersId",
                table: "OrderShop",
                newName: "OrderId");

            migrationBuilder.RenameIndex(
                name: "IX_OrderShop_ShopsId",
                table: "OrderShop",
                newName: "IX_OrderShop_ShopId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderShop_Order_OrderId",
                table: "OrderShop",
                column: "OrderId",
                principalTable: "Order",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderShop_Shop_ShopId",
                table: "OrderShop",
                column: "ShopId",
                principalTable: "Shop",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderShop_Order_OrderId",
                table: "OrderShop");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderShop_Shop_ShopId",
                table: "OrderShop");

            migrationBuilder.RenameColumn(
                name: "ShopId",
                table: "OrderShop",
                newName: "ShopsId");

            migrationBuilder.RenameColumn(
                name: "OrderId",
                table: "OrderShop",
                newName: "OrdersId");

            migrationBuilder.RenameIndex(
                name: "IX_OrderShop_ShopId",
                table: "OrderShop",
                newName: "IX_OrderShop_ShopsId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderShop_Order_OrdersId",
                table: "OrderShop",
                column: "OrdersId",
                principalTable: "Order",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderShop_Shop_ShopsId",
                table: "OrderShop",
                column: "ShopsId",
                principalTable: "Shop",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
