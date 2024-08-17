using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DATN.API.Migrations
{
    /// <inheritdoc />
    public partial class ver164 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_CategoryDetails_IdCategoryProduct",
                table: "Products");

            migrationBuilder.DropTable(
                name: "ImagePets");

            migrationBuilder.DropColumn(
                name: "MaxWeight",
                table: "ServiceDetails");

            migrationBuilder.DropColumn(
                name: "MinWeight",
                table: "ServiceDetails");

            migrationBuilder.DropColumn(
                name: "OriginalColor",
                table: "Pets");

            migrationBuilder.DropColumn(
                name: "Vaccinated",
                table: "Pets");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "ServiceDetails",
                newName: "NameDetail");

            migrationBuilder.RenameColumn(
                name: "IdCategoryProduct",
                table: "Products",
                newName: "IdCategoryDeatail");

            migrationBuilder.RenameIndex(
                name: "IX_Products_IdCategoryProduct",
                table: "Products",
                newName: "IX_Products_IdCategoryDeatail");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_CategoryDetails_IdCategoryDeatail",
                table: "Products",
                column: "IdCategoryDeatail",
                principalTable: "CategoryDetails",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_CategoryDetails_IdCategoryDeatail",
                table: "Products");

            migrationBuilder.RenameColumn(
                name: "NameDetail",
                table: "ServiceDetails",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "IdCategoryDeatail",
                table: "Products",
                newName: "IdCategoryProduct");

            migrationBuilder.RenameIndex(
                name: "IX_Products_IdCategoryDeatail",
                table: "Products",
                newName: "IX_Products_IdCategoryProduct");

            migrationBuilder.AddColumn<float>(
                name: "MaxWeight",
                table: "ServiceDetails",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "MinWeight",
                table: "ServiceDetails",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<string>(
                name: "OriginalColor",
                table: "Pets",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Vaccinated",
                table: "Pets",
                type: "bit",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ImagePets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PetId = table.Column<int>(type: "int", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDefault = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImagePets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ImagePets_Pets_PetId",
                        column: x => x.PetId,
                        principalTable: "Pets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ImagePets_PetId",
                table: "ImagePets",
                column: "PetId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_CategoryDetails_IdCategoryProduct",
                table: "Products",
                column: "IdCategoryProduct",
                principalTable: "CategoryDetails",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
