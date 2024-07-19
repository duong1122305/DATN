using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DATN.API.Migrations
{
    /// <inheritdoc />
    public partial class no544 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CategoryProducts_Categories_IdCategory",
                table: "CategoryProducts");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_CategoryProducts_IdCategoryProduct",
                table: "Products");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CategoryProducts",
                table: "CategoryProducts");

            migrationBuilder.RenameTable(
                name: "CategoryProducts",
                newName: "CategoryDetails");

            migrationBuilder.RenameIndex(
                name: "IX_CategoryProducts_IdCategory",
                table: "CategoryDetails",
                newName: "IX_CategoryDetails_IdCategory");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CategoryDetails",
                table: "CategoryDetails",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CategoryDetails_Categories_IdCategory",
                table: "CategoryDetails",
                column: "IdCategory",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_CategoryDetails_IdCategoryProduct",
                table: "Products",
                column: "IdCategoryProduct",
                principalTable: "CategoryDetails",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CategoryDetails_Categories_IdCategory",
                table: "CategoryDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_CategoryDetails_IdCategoryProduct",
                table: "Products");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CategoryDetails",
                table: "CategoryDetails");

            migrationBuilder.RenameTable(
                name: "CategoryDetails",
                newName: "CategoryProducts");

            migrationBuilder.RenameIndex(
                name: "IX_CategoryDetails_IdCategory",
                table: "CategoryProducts",
                newName: "IX_CategoryProducts_IdCategory");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CategoryProducts",
                table: "CategoryProducts",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CategoryProducts_Categories_IdCategory",
                table: "CategoryProducts",
                column: "IdCategory",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_CategoryProducts_IdCategoryProduct",
                table: "Products",
                column: "IdCategoryProduct",
                principalTable: "CategoryProducts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
