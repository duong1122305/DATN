using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DATN.API.Migrations
{
    /// <inheritdoc />
    public partial class ver16 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reports_BookingDetails_BookingDetailId",
                table: "Reports");

            migrationBuilder.DropIndex(
                name: "IX_Reports_BookingDetailId",
                table: "Reports");

            migrationBuilder.RenameColumn(
                name: "BookingDetailId",
                table: "Reports",
                newName: "Rate");

            migrationBuilder.AddColumn<int>(
                name: "BookingId",
                table: "Reports",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "BookingViews",
                columns: table => new
                {
                    IdGuest = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IdBooking = table.Column<int>(type: "int", nullable: true),
                    NameGuest = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    BookingTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsPayment = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateIndex(
                name: "IX_Reports_BookingId",
                table: "Reports",
                column: "BookingId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reports_BookingDetails_BookingId",
                table: "Reports",
                column: "BookingId",
                principalTable: "BookingDetails",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reports_BookingDetails_BookingId",
                table: "Reports");

            migrationBuilder.DropTable(
                name: "BookingViews");

            migrationBuilder.DropIndex(
                name: "IX_Reports_BookingId",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "BookingId",
                table: "Reports");

            migrationBuilder.RenameColumn(
                name: "Rate",
                table: "Reports",
                newName: "BookingDetailId");

            migrationBuilder.CreateIndex(
                name: "IX_Reports_BookingDetailId",
                table: "Reports",
                column: "BookingDetailId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reports_BookingDetails_BookingDetailId",
                table: "Reports",
                column: "BookingDetailId",
                principalTable: "BookingDetails",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
