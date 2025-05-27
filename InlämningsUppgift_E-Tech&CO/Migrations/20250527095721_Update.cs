using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InlämningsUppgift_E_Tech_CO.Migrations
{
    /// <inheritdoc />
    public partial class Update : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductCategory_Shop_ShopId",
                table: "ProductCategory");

            migrationBuilder.DropIndex(
                name: "IX_ProductCategory_ShopId",
                table: "ProductCategory");

            migrationBuilder.DropColumn(
                name: "ShopId",
                table: "ProductCategory");

            migrationBuilder.CreateTable(
                name: "ProductCategoryShop",
                columns: table => new
                {
                    ProductCategoriesProductCategoryId = table.Column<int>(type: "int", nullable: false),
                    ShopId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductCategoryShop", x => new { x.ProductCategoriesProductCategoryId, x.ShopId });
                    table.ForeignKey(
                        name: "FK_ProductCategoryShop_ProductCategory_ProductCategoriesProductCategoryId",
                        column: x => x.ProductCategoriesProductCategoryId,
                        principalTable: "ProductCategory",
                        principalColumn: "ProductCategoryId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductCategoryShop_Shop_ShopId",
                        column: x => x.ShopId,
                        principalTable: "Shop",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductCategoryShop_ShopId",
                table: "ProductCategoryShop",
                column: "ShopId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductCategoryShop");

            migrationBuilder.AddColumn<int>(
                name: "ShopId",
                table: "ProductCategory",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductCategory_ShopId",
                table: "ProductCategory",
                column: "ShopId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductCategory_Shop_ShopId",
                table: "ProductCategory",
                column: "ShopId",
                principalTable: "Shop",
                principalColumn: "Id");
        }
    }
}
