using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InlämningsUppgift_E_Tech_CO.Migrations
{
    /// <inheritdoc />
    public partial class things : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrderHistoryShop");

            migrationBuilder.AddColumn<int>(
                name: "OrderHistoryId",
                table: "Shop",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Shop_OrderHistoryId",
                table: "Shop",
                column: "OrderHistoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Shop_OrderHistories_OrderHistoryId",
                table: "Shop",
                column: "OrderHistoryId",
                principalTable: "OrderHistories",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Shop_OrderHistories_OrderHistoryId",
                table: "Shop");

            migrationBuilder.DropIndex(
                name: "IX_Shop_OrderHistoryId",
                table: "Shop");

            migrationBuilder.DropColumn(
                name: "OrderHistoryId",
                table: "Shop");

            migrationBuilder.CreateTable(
                name: "OrderHistoryShop",
                columns: table => new
                {
                    OrderHistoryId = table.Column<int>(type: "int", nullable: false),
                    ShopId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderHistoryShop", x => new { x.OrderHistoryId, x.ShopId });
                    table.ForeignKey(
                        name: "FK_OrderHistoryShop_OrderHistories_OrderHistoryId",
                        column: x => x.OrderHistoryId,
                        principalTable: "OrderHistories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderHistoryShop_Shop_ShopId",
                        column: x => x.ShopId,
                        principalTable: "Shop",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrderHistoryShop_ShopId",
                table: "OrderHistoryShop",
                column: "ShopId");
        }
    }
}
