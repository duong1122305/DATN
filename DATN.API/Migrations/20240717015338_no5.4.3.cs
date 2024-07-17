using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DATN.API.Migrations
{
    /// <inheritdoc />
    public partial class no543 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "url",
                table: "AspNetUsers",
                newName: "ImgUrl");

            migrationBuilder.AddColumn<string>(
                name: "ImgID",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImgID",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "ImgUrl",
                table: "AspNetUsers",
                newName: "url");
        }
    }
}
