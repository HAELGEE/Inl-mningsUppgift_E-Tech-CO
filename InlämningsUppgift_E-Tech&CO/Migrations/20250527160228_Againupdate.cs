using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InlämningsUppgift_E_Tech_CO.Migrations
{
    /// <inheritdoc />
    public partial class Againupdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductSubcategory_ProductCategory_ProductCategoryId",
                table: "ProductSubcategory");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductSubcategory_ProductCategory_ProductCategoryId",
                table: "ProductSubcategory",
                column: "ProductCategoryId",
                principalTable: "ProductCategory",
                principalColumn: "ProductCategoryId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductSubcategory_ProductCategory_ProductCategoryId",
                table: "ProductSubcategory");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductSubcategory_ProductCategory_ProductCategoryId",
                table: "ProductSubcategory",
                column: "ProductCategoryId",
                principalTable: "ProductCategory",
                principalColumn: "ProductCategoryId");
        }
    }
}
