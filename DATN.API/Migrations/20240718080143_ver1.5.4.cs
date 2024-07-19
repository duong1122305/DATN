using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DATN.API.Migrations
{
    /// <inheritdoc />
    public partial class ver154 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductDetails_PetTypes_IdPetType",
                table: "ProductDetails");

            migrationBuilder.RenameColumn(
                name: "IdPetType",
                table: "ProductDetails",
                newName: "PetTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductDetails_IdPetType",
                table: "ProductDetails",
                newName: "IX_ProductDetails_PetTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductDetails_PetTypes_PetTypeId",
                table: "ProductDetails",
                column: "PetTypeId",
                principalTable: "PetTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductDetails_PetTypes_PetTypeId",
                table: "ProductDetails");

            migrationBuilder.RenameColumn(
                name: "PetTypeId",
                table: "ProductDetails",
                newName: "IdPetType");

            migrationBuilder.RenameIndex(
                name: "IX_ProductDetails_PetTypeId",
                table: "ProductDetails",
                newName: "IX_ProductDetails_IdPetType");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductDetails_PetTypes_IdPetType",
                table: "ProductDetails",
                column: "IdPetType",
                principalTable: "PetTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
