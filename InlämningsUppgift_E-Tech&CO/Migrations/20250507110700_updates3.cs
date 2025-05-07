using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InlämningsUppgift_E_Tech_CO.Migrations
{
    /// <inheritdoc />
    public partial class updates3 : Migration
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

            migrationBuilder.RenameColumn(
                name: "PriceAtPurchase",
                table: "Shop",
                newName: "Price");

            migrationBuilder.RenameColumn(
                name: "Price",
                table: "Order",
                newName: "PriceAtPurchase");

            migrationBuilder.CreateTable(
                name: "OrderShop",
                columns: table => new
                {
                    OrdersId = table.Column<int>(type: "int", nullable: false),
                    ShopsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderShop", x => new { x.OrdersId, x.ShopsId });
                    table.ForeignKey(
                        name: "FK_OrderShop_Order_OrdersId",
                        column: x => x.OrdersId,
                        principalTable: "Order",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderShop_Shop_ShopsId",
                        column: x => x.ShopsId,
                        principalTable: "Shop",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrderShop_ShopsId",
                table: "OrderShop",
                column: "ShopsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrderShop");

            migrationBuilder.RenameColumn(
                name: "Price",
                table: "Shop",
                newName: "PriceAtPurchase");

            migrationBuilder.RenameColumn(
                name: "PriceAtPurchase",
                table: "Order",
                newName: "Price");

            migrationBuilder.AddColumn<int>(
                name: "ShopId",
                table: "Order",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Order_ShopId",
                table: "Order",
                column: "ShopId");

            migrationBuilder.AddForeignKey(
                name: "FK_Order_Shop_ShopId",
                table: "Order",
                column: "ShopId",
                principalTable: "Shop",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
