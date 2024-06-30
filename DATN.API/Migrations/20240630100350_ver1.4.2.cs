using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DATN.API.Migrations
{
    /// <inheritdoc />
    public partial class ver142 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_CategoryProducts_CategoryProductIdCategory",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_CategoryProductIdCategory",
                table: "Products");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_CategoryProducts_IdCategory",
                table: "CategoryProducts");

            migrationBuilder.DropColumn(
                name: "CategoryProductIdCategory",
                table: "Products");

            migrationBuilder.CreateIndex(
                name: "IX_Products_IdCategoryProduct",
                table: "Products",
                column: "IdCategoryProduct");

            migrationBuilder.CreateIndex(
                name: "IX_CategoryProducts_IdCategory",
                table: "CategoryProducts",
                column: "IdCategory");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_CategoryProducts_IdCategoryProduct",
                table: "Products",
                column: "IdCategoryProduct",
                principalTable: "CategoryProducts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_CategoryProducts_IdCategoryProduct",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_IdCategoryProduct",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_CategoryProducts_IdCategory",
                table: "CategoryProducts");

            migrationBuilder.AddColumn<int>(
                name: "CategoryProductIdCategory",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddUniqueConstraint(
                name: "AK_CategoryProducts_IdCategory",
                table: "CategoryProducts",
                column: "IdCategory");

            migrationBuilder.CreateIndex(
                name: "IX_Products_CategoryProductIdCategory",
                table: "Products",
                column: "CategoryProductIdCategory");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_CategoryProducts_CategoryProductIdCategory",
                table: "Products",
                column: "CategoryProductIdCategory",
                principalTable: "CategoryProducts",
                principalColumn: "IdCategory",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
