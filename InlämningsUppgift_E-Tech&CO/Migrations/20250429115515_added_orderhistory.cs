using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InlämningsUppgift_E_Tech_CO.Migrations
{
    /// <inheritdoc />
    public partial class added_orderhistory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OrderHistoryId",
                table: "Shop",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "OrderHistory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerId = table.Column<int>(type: "int", nullable: false),
                    ShopId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderHistory", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CustomerOrderHistory",
                columns: table => new
                {
                    CustomerId = table.Column<int>(type: "int", nullable: false),
                    OrderHistoryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerOrderHistory", x => new { x.CustomerId, x.OrderHistoryId });
                    table.ForeignKey(
                        name: "FK_CustomerOrderHistory_Customer_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CustomerOrderHistory_OrderHistory_OrderHistoryId",
                        column: x => x.OrderHistoryId,
                        principalTable: "OrderHistory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Shop_OrderHistoryId",
                table: "Shop",
                column: "OrderHistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerOrderHistory_OrderHistoryId",
                table: "CustomerOrderHistory",
                column: "OrderHistoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Shop_OrderHistory_OrderHistoryId",
                table: "Shop",
                column: "OrderHistoryId",
                principalTable: "OrderHistory",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Shop_OrderHistory_OrderHistoryId",
                table: "Shop");

            migrationBuilder.DropTable(
                name: "CustomerOrderHistory");

            migrationBuilder.DropTable(
                name: "OrderHistory");

            migrationBuilder.DropIndex(
                name: "IX_Shop_OrderHistoryId",
                table: "Shop");

            migrationBuilder.DropColumn(
                name: "OrderHistoryId",
                table: "Shop");
        }
    }
}
