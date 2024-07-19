using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DATN.API.Migrations
{
    /// <inheritdoc />
    public partial class ver155 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ImageProducts_ProductDetails_IdProductDetail",
                table: "ImageProducts");

            migrationBuilder.RenameColumn(
                name: "IdProductDetail",
                table: "ImageProducts",
                newName: "ProductID");

            migrationBuilder.RenameIndex(
                name: "IX_ImageProducts_IdProductDetail",
                table: "ImageProducts",
                newName: "IX_ImageProducts_ProductID");

            migrationBuilder.AddForeignKey(
                name: "FK_ImageProducts_Products_ProductID",
                table: "ImageProducts",
                column: "ProductID",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ImageProducts_Products_ProductID",
                table: "ImageProducts");

            migrationBuilder.RenameColumn(
                name: "ProductID",
                table: "ImageProducts",
                newName: "IdProductDetail");

            migrationBuilder.RenameIndex(
                name: "IX_ImageProducts_ProductID",
                table: "ImageProducts",
                newName: "IX_ImageProducts_IdProductDetail");

            migrationBuilder.AddColumn<int>(
                name: "PetTypeId",
                table: "ProductDetails",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ProductDetails_PetTypeId",
                table: "ProductDetails",
                column: "PetTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_ImageProducts_ProductDetails_IdProductDetail",
                table: "ImageProducts",
                column: "IdProductDetail",
                principalTable: "ProductDetails",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductDetails_PetTypes_PetTypeId",
                table: "ProductDetails",
                column: "PetTypeId",
                principalTable: "PetTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
