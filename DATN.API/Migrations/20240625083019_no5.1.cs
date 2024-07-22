using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DATN.API.Migrations
{
    /// <inheritdoc />
    public partial class no51 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_AspNetUsers_StaffAtCounterId",
                table: "Bookings");

            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_AspNetUsers_StaffConfirmId",
                table: "Bookings");

            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_AspNetUsers_StaffUpdateId",
                table: "Bookings");

            migrationBuilder.DropIndex(
                name: "IX_Bookings_PaymentTypeId",
                table: "Bookings");

            migrationBuilder.DropIndex(
                name: "IX_Bookings_StaffAtCounterId",
                table: "Bookings");

            migrationBuilder.DropIndex(
                name: "IX_Bookings_StaffConfirmId",
                table: "Bookings");

            migrationBuilder.DropIndex(
                name: "IX_Bookings_StaffUpdateId",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "ConfirmedTime",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "CustomerArrivalTime",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "DesiredStartTime",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "StaffAtCounterId",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "StaffConfirmId",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "StaffUpdateId",
                table: "Bookings");

            migrationBuilder.RenameColumn(
                name: "UpdatedBy",
                table: "Bookings",
                newName: "UserId1");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "Bookings",
                newName: "AppointmentTime");

            migrationBuilder.AddColumn<bool>(
                name: "IsAddToSchedule",
                table: "Bookings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "Bookings",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ActionBooking",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActionBooking", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "HistoryAction",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ActionID = table.Column<int>(type: "int", nullable: false),
                    IdBooking = table.Column<int>(type: "int", nullable: false),
                    ByGuest = table.Column<bool>(type: "bit", nullable: false),
                    ActionByID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ActionTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ActionBookingID = table.Column<int>(type: "int", nullable: false),
                    BookingId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HistoryAction", x => x.ID);
                    table.ForeignKey(
                        name: "FK_HistoryAction_ActionBooking_ActionBookingID",
                        column: x => x.ActionBookingID,
                        principalTable: "ActionBooking",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HistoryAction_AspNetUsers_ActionByID",
                        column: x => x.ActionByID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_HistoryAction_Bookings_BookingId",
                        column: x => x.BookingId,
                        principalTable: "Bookings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_PaymentTypeId",
                table: "Bookings",
                column: "PaymentTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_UserId",
                table: "Bookings",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_UserId1",
                table: "Bookings",
                column: "UserId1");

            migrationBuilder.CreateIndex(
                name: "IX_HistoryAction_ActionBookingID",
                table: "HistoryAction",
                column: "ActionBookingID");

            migrationBuilder.CreateIndex(
                name: "IX_HistoryAction_ActionByID",
                table: "HistoryAction",
                column: "ActionByID");

            migrationBuilder.CreateIndex(
                name: "IX_HistoryAction_BookingId",
                table: "HistoryAction",
                column: "BookingId");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_AspNetUsers_UserId",
                table: "Bookings",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_AspNetUsers_UserId1",
                table: "Bookings",
                column: "UserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_AspNetUsers_UserId",
                table: "Bookings");

            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_AspNetUsers_UserId1",
                table: "Bookings");

            migrationBuilder.DropTable(
                name: "HistoryAction");

            migrationBuilder.DropTable(
                name: "ActionBooking");

            migrationBuilder.DropIndex(
                name: "IX_Bookings_PaymentTypeId",
                table: "Bookings");

            migrationBuilder.DropIndex(
                name: "IX_Bookings_UserId",
                table: "Bookings");

            migrationBuilder.DropIndex(
                name: "IX_Bookings_UserId1",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "IsAddToSchedule",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Bookings");

            migrationBuilder.RenameColumn(
                name: "UserId1",
                table: "Bookings",
                newName: "UpdatedBy");

            migrationBuilder.RenameColumn(
                name: "AppointmentTime",
                table: "Bookings",
                newName: "UpdatedAt");

            migrationBuilder.AddColumn<DateTime>(
                name: "ConfirmedTime",
                table: "Bookings",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CustomerArrivalTime",
                table: "Bookings",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DesiredStartTime",
                table: "Bookings",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "StaffAtCounterId",
                table: "Bookings",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "StaffConfirmId",
                table: "Bookings",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "StaffUpdateId",
                table: "Bookings",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_PaymentTypeId",
                table: "Bookings",
                column: "PaymentTypeId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_StaffAtCounterId",
                table: "Bookings",
                column: "StaffAtCounterId");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_StaffConfirmId",
                table: "Bookings",
                column: "StaffConfirmId");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_StaffUpdateId",
                table: "Bookings",
                column: "StaffUpdateId");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_AspNetUsers_StaffAtCounterId",
                table: "Bookings",
                column: "StaffAtCounterId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_AspNetUsers_StaffConfirmId",
                table: "Bookings",
                column: "StaffConfirmId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_AspNetUsers_StaffUpdateId",
                table: "Bookings",
                column: "StaffUpdateId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
