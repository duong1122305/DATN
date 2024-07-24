using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DATN.API.Migrations
{
    /// <inheritdoc />
    public partial class ver1451 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeAttendances_AspNetUsers_UserId",
                table: "EmployeeAttendances");

            migrationBuilder.DropIndex(
                name: "IX_EmployeeAttendances_UserId",
                table: "EmployeeAttendances");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "EmployeeAttendances");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "EmployeeAttendances",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeAttendances_UserId",
                table: "EmployeeAttendances",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeAttendances_AspNetUsers_UserId",
                table: "EmployeeAttendances",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
