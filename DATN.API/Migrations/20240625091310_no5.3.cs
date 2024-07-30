using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DATN.API.Migrations
{
    /// <inheritdoc />
    public partial class no53 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HistoryAction_ActionBooking_ActionBookingID",
                table: "HistoryAction");

            migrationBuilder.DropForeignKey(
                name: "FK_HistoryAction_AspNetUsers_ActionById",
                table: "HistoryAction");

            migrationBuilder.DropForeignKey(
                name: "FK_HistoryAction_Bookings_BookingId",
                table: "HistoryAction");

            migrationBuilder.DropIndex(
                name: "IX_HistoryAction_ActionBookingID",
                table: "HistoryAction");

            migrationBuilder.DropColumn(
                name: "ActionBookingID",
                table: "HistoryAction");

            migrationBuilder.DropColumn(
                name: "ActionByStaff",
                table: "HistoryAction");

            migrationBuilder.DropColumn(
                name: "IdBooking",
                table: "HistoryAction");

            migrationBuilder.RenameColumn(
                name: "BookingId",
                table: "HistoryAction",
                newName: "BookingID");

            migrationBuilder.RenameColumn(
                name: "ActionById",
                table: "HistoryAction",
                newName: "ActionByID");

            migrationBuilder.RenameIndex(
                name: "IX_HistoryAction_BookingId",
                table: "HistoryAction",
                newName: "IX_HistoryAction_BookingID");

            migrationBuilder.RenameIndex(
                name: "IX_HistoryAction_ActionById",
                table: "HistoryAction",
                newName: "IX_HistoryAction_ActionByID");

            migrationBuilder.AlterColumn<Guid>(
                name: "ActionByID",
                table: "HistoryAction",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.CreateIndex(
                name: "IX_HistoryAction_ActionID",
                table: "HistoryAction",
                column: "ActionID");

            migrationBuilder.AddForeignKey(
                name: "FK_HistoryAction_ActionBooking_ActionID",
                table: "HistoryAction",
                column: "ActionID",
                principalTable: "ActionBooking",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_HistoryAction_AspNetUsers_ActionByID",
                table: "HistoryAction",
                column: "ActionByID",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_HistoryAction_Bookings_BookingID",
                table: "HistoryAction",
                column: "BookingID",
                principalTable: "Bookings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HistoryAction_ActionBooking_ActionID",
                table: "HistoryAction");

            migrationBuilder.DropForeignKey(
                name: "FK_HistoryAction_AspNetUsers_ActionByID",
                table: "HistoryAction");

            migrationBuilder.DropForeignKey(
                name: "FK_HistoryAction_Bookings_BookingID",
                table: "HistoryAction");

            migrationBuilder.DropIndex(
                name: "IX_HistoryAction_ActionID",
                table: "HistoryAction");

            migrationBuilder.RenameColumn(
                name: "BookingID",
                table: "HistoryAction",
                newName: "BookingId");

            migrationBuilder.RenameColumn(
                name: "ActionByID",
                table: "HistoryAction",
                newName: "ActionById");

            migrationBuilder.RenameIndex(
                name: "IX_HistoryAction_BookingID",
                table: "HistoryAction",
                newName: "IX_HistoryAction_BookingId");

            migrationBuilder.RenameIndex(
                name: "IX_HistoryAction_ActionByID",
                table: "HistoryAction",
                newName: "IX_HistoryAction_ActionById");

            migrationBuilder.AlterColumn<Guid>(
                name: "ActionById",
                table: "HistoryAction",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ActionBookingID",
                table: "HistoryAction",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "ActionByStaff",
                table: "HistoryAction",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "IdBooking",
                table: "HistoryAction",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_HistoryAction_ActionBookingID",
                table: "HistoryAction",
                column: "ActionBookingID");

            migrationBuilder.AddForeignKey(
                name: "FK_HistoryAction_ActionBooking_ActionBookingID",
                table: "HistoryAction",
                column: "ActionBookingID",
                principalTable: "ActionBooking",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_HistoryAction_AspNetUsers_ActionById",
                table: "HistoryAction",
                column: "ActionById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_HistoryAction_Bookings_BookingId",
                table: "HistoryAction",
                column: "BookingId",
                principalTable: "Bookings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
