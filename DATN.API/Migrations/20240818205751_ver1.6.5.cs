using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DATN.API.Migrations
{
    /// <inheritdoc />
    public partial class ver165 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ComboId",
                table: "BookingDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkShifts_EmployeeSchedules_EmployeeScheduleId",
                table: "WorkShifts"
                );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ComboId",
                table: "BookingDetails",
                type: "int",
                nullable: true);
        }
    }
}
