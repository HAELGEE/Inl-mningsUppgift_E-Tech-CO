using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InlämningsUppgift_E_Tech_CO.Migrations
{
    /// <inheritdoc />
    public partial class updates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Order_shipping_ShippingId",
                table: "Order");

            migrationBuilder.DropForeignKey(
                name: "FK_Shop_Order_OrderId",
                table: "Shop");

            migrationBuilder.DropTable(
                name: "OrderShoppingCart");

            migrationBuilder.DropTable(
                name: "ShoppingCart");

            migrationBuilder.DropIndex(
                name: "IX_Shop_OrderId",
                table: "Shop");

            migrationBuilder.DropIndex(
                name: "IX_Order_ShippingId",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "OrderId",
                table: "Shop");

            migrationBuilder.DropColumn(
                name: "ShippingId",
                table: "Order");

            migrationBuilder.RenameColumn(
                name: "Stock",
                table: "Shop",
                newName: "Quantity");

            migrationBuilder.RenameColumn(
                name: "Price",
                table: "Shop",
                newName: "PriceAtPurchase");

            migrationBuilder.AddColumn<int>(
                name: "ShopId",
                table: "Order",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_shipping_OrderId",
                table: "shipping",
                column: "OrderId",
                unique: true,
                filter: "[OrderId] IS NOT NULL");

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

            migrationBuilder.AddForeignKey(
                name: "FK_shipping_Order_OrderId",
                table: "shipping",
                column: "OrderId",
                principalTable: "Order",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Order_Shop_ShopId",
                table: "Order");

            migrationBuilder.DropForeignKey(
                name: "FK_shipping_Order_OrderId",
                table: "shipping");

            migrationBuilder.DropIndex(
                name: "IX_shipping_OrderId",
                table: "shipping");

            migrationBuilder.DropIndex(
                name: "IX_Order_ShopId",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "ShopId",
                table: "Order");

            migrationBuilder.RenameColumn(
                name: "Quantity",
                table: "Shop",
                newName: "Stock");

            migrationBuilder.RenameColumn(
                name: "PriceAtPurchase",
                table: "Shop",
                newName: "Price");

            migrationBuilder.AddColumn<int>(
                name: "OrderId",
                table: "Shop",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ShippingId",
                table: "Order",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ShoppingCart",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TotalItems = table.Column<int>(type: "int", nullable: true),
                    TotalPrice = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShoppingCart", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OrderShoppingCart",
                columns: table => new
                {
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    ShoppingCartId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderShoppingCart", x => new { x.OrderId, x.ShoppingCartId });
                    table.ForeignKey(
                        name: "FK_OrderShoppingCart_Order_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Order",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderShoppingCart_ShoppingCart_ShoppingCartId",
                        column: x => x.ShoppingCartId,
                        principalTable: "ShoppingCart",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Shop_OrderId",
                table: "Shop",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_Order_ShippingId",
                table: "Order",
                column: "ShippingId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderShoppingCart_ShoppingCartId",
                table: "OrderShoppingCart",
                column: "ShoppingCartId");

            migrationBuilder.AddForeignKey(
                name: "FK_Order_shipping_ShippingId",
                table: "Order",
                column: "ShippingId",
                principalTable: "shipping",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Shop_Order_OrderId",
                table: "Shop",
                column: "OrderId",
                principalTable: "Order",
                principalColumn: "Id");
        }
    }
}
