using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DATN.API.Migrations
{
    /// <inheritdoc />
    public partial class addAvatarGuest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AvatarUrl",
                table: "Guests",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AvatarUrl",
                table: "Guests");
        }
    }
}
