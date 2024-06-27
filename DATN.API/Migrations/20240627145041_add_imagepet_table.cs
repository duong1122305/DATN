using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DATN.API.Migrations
{
    /// <inheritdoc />
    public partial class add_imagepet_table : Migration
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
                name: "DeleteAt",
                table: "ServiceDetails");

            migrationBuilder.DropColumn(
                name: "Deleted",
                table: "ServiceDetails");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "ServiceDetails");

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

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "Bookings");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "Bookings",
                newName: "AppointmentTime");

            migrationBuilder.AddColumn<string>(
                name: "Desciption",
                table: "Services",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "MaxWeight",
                table: "ServiceDetails",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "MinWeight",
                table: "ServiceDetails",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<bool>(
                name: "IsAddToSchedule",
                table: "Bookings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "ActionBookings",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActionBookings", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "ImagePets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDefault = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    PetId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImagePets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ImagePets_Pets_PetId",
                        column: x => x.PetId,
                        principalTable: "Pets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HistoryActions",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ActionID = table.Column<int>(type: "int", nullable: false),
                    BookingID = table.Column<int>(type: "int", nullable: false),
                    ByGuest = table.Column<bool>(type: "bit", nullable: false),
                    ActionByID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ActionTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HistoryActions", x => x.ID);
                    table.ForeignKey(
                        name: "FK_HistoryActions_ActionBookings_ActionID",
                        column: x => x.ActionID,
                        principalTable: "ActionBookings",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HistoryActions_AspNetUsers_ActionByID",
                        column: x => x.ActionByID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_HistoryActions_Bookings_BookingID",
                        column: x => x.BookingID,
                        principalTable: "Bookings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_PaymentTypeId",
                table: "Bookings",
                column: "PaymentTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_HistoryActions_ActionByID",
                table: "HistoryActions",
                column: "ActionByID");

            migrationBuilder.CreateIndex(
                name: "IX_HistoryActions_ActionID",
                table: "HistoryActions",
                column: "ActionID");

            migrationBuilder.CreateIndex(
                name: "IX_HistoryActions_BookingID",
                table: "HistoryActions",
                column: "BookingID");

            migrationBuilder.CreateIndex(
                name: "IX_ImagePets_PetId",
                table: "ImagePets",
                column: "PetId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HistoryActions");

            migrationBuilder.DropTable(
                name: "ImagePets");

            migrationBuilder.DropTable(
                name: "ActionBookings");

            migrationBuilder.DropIndex(
                name: "IX_Bookings_PaymentTypeId",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "Desciption",
                table: "Services");

            migrationBuilder.DropColumn(
                name: "MaxWeight",
                table: "ServiceDetails");

            migrationBuilder.DropColumn(
                name: "MinWeight",
                table: "ServiceDetails");

            migrationBuilder.DropColumn(
                name: "IsAddToSchedule",
                table: "Bookings");

            migrationBuilder.RenameColumn(
                name: "AppointmentTime",
                table: "Bookings",
                newName: "UpdatedAt");

            migrationBuilder.AddColumn<DateTime>(
                name: "DeleteAt",
                table: "ServiceDetails",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Deleted",
                table: "ServiceDetails",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "ServiceDetails",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

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

            migrationBuilder.AddColumn<Guid>(
                name: "UpdatedBy",
                table: "Bookings",
                type: "uniqueidentifier",
                nullable: true);

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
