using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DATN.API.Migrations
{
    /// <inheritdoc />
    public partial class ver14 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                table: "PetSpecies");

            migrationBuilder.DropColumn(
                name: "IsGuest",
                table: "Pets");

            migrationBuilder.AddColumn<bool>(
                name: "IsDelete",
                table: "PetSpecies",
                type: "bit",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDelete",
                table: "PetSpecies");

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "PetSpecies",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsGuest",
                table: "Pets",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
