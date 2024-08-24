using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DATN.API.Migrations
{
    /// <inheritdoc />
    public partial class ver167 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reports_BookingDetails_BookingId",
                table: "Reports");

            migrationBuilder.AddColumn<int>(
                name: "CountBookingDetail",
                table: "BookingViews",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_Reports_Bookings_BookingId",
                table: "Reports",
                column: "BookingId",
                principalTable: "Bookings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reports_Bookings_BookingId",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "CountBookingDetail",
                table: "BookingViews");

            migrationBuilder.AddForeignKey(
                name: "FK_Reports_BookingDetails_BookingId",
                table: "Reports",
                column: "BookingId",
                principalTable: "BookingDetails",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
