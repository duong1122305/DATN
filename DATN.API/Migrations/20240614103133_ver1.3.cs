using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DATN.API.Migrations
{
    /// <inheritdoc />



    public partial class ver13 : Migration

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

            migrationBuilder.AddColumn<string>(
                name: "VoucherName",
                table: "Discounts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.DropColumn(
                name: "VoucherName",
                table: "Discounts");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "AspNetUsers",
                type: "bit",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit");

        }
    }
}
