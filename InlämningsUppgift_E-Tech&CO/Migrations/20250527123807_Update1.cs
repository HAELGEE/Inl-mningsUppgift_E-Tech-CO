using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InlämningsUppgift_E_Tech_CO.Migrations
{
    /// <inheritdoc />
    public partial class Update1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductSubCategory_ProductCategory_ProductCategoryId",
                table: "ProductSubCategory");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductSubCategory",
                table: "ProductSubCategory");

            migrationBuilder.RenameTable(
                name: "ProductSubCategory",
                newName: "ProductSubcategory");

            migrationBuilder.RenameIndex(
                name: "IX_ProductSubCategory_ProductCategoryId",
                table: "ProductSubcategory",
                newName: "IX_ProductSubcategory_ProductCategoryId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductSubcategory",
                table: "ProductSubcategory",
                column: "ProductSubcategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductSubcategory_ProductCategory_ProductCategoryId",
                table: "ProductSubcategory",
                column: "ProductCategoryId",
                principalTable: "ProductCategory",
                principalColumn: "ProductCategoryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductSubcategory_ProductCategory_ProductCategoryId",
                table: "ProductSubcategory");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductSubcategory",
                table: "ProductSubcategory");

            migrationBuilder.RenameTable(
                name: "ProductSubcategory",
                newName: "ProductSubCategory");

            migrationBuilder.RenameIndex(
                name: "IX_ProductSubcategory_ProductCategoryId",
                table: "ProductSubCategory",
                newName: "IX_ProductSubCategory_ProductCategoryId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductSubCategory",
                table: "ProductSubCategory",
                column: "ProductSubcategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductSubCategory_ProductCategory_ProductCategoryId",
                table: "ProductSubCategory",
                column: "ProductCategoryId",
                principalTable: "ProductCategory",
                principalColumn: "ProductCategoryId");
        }
    }
}
