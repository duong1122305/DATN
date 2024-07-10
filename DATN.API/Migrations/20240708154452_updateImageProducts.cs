using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DATN.API.Migrations
{
    /// <inheritdoc />
    public partial class updateImageProducts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ImageProducts_Products_IdProduct",
                table: "ImageProducts");

            migrationBuilder.RenameColumn(
                name: "IdProduct",
                table: "ImageProducts",
                newName: "IdProductDetail");

            migrationBuilder.RenameIndex(
                name: "IX_ImageProducts_IdProduct",
                table: "ImageProducts",
                newName: "IX_ImageProducts_IdProductDetail");

            migrationBuilder.AddForeignKey(
                name: "FK_ImageProducts_ProductDetails_IdProductDetail",
                table: "ImageProducts",
                column: "IdProductDetail",
                principalTable: "ProductDetails",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ImageProducts_ProductDetails_IdProductDetail",
                table: "ImageProducts");

            migrationBuilder.RenameColumn(
                name: "IdProductDetail",
                table: "ImageProducts",
                newName: "IdProduct");

            migrationBuilder.RenameIndex(
                name: "IX_ImageProducts_IdProductDetail",
                table: "ImageProducts",
                newName: "IX_ImageProducts_IdProduct");

            migrationBuilder.AddForeignKey(
                name: "FK_ImageProducts_Products_IdProduct",
                table: "ImageProducts",
                column: "IdProduct",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
