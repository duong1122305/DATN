using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DATN.API.Migrations
{
    /// <inheritdoc />
    public partial class no54 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropPrimaryKey(
                name: "PK_HistoryAction",
                table: "HistoryAction");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ActionBooking",
                table: "ActionBooking");

            migrationBuilder.DropColumn(
                name: "DeleteAt",
                table: "ServiceDetails");

            migrationBuilder.DropColumn(
                name: "Deleted",
                table: "ServiceDetails");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "ServiceDetails");

            migrationBuilder.RenameTable(
                name: "HistoryAction",
                newName: "HistoryActions");

            migrationBuilder.RenameTable(
                name: "ActionBooking",
                newName: "ActionBookings");

            migrationBuilder.RenameIndex(
                name: "IX_HistoryAction_BookingID",
                table: "HistoryActions",
                newName: "IX_HistoryActions_BookingID");

            migrationBuilder.RenameIndex(
                name: "IX_HistoryAction_ActionID",
                table: "HistoryActions",
                newName: "IX_HistoryActions_ActionID");

            migrationBuilder.RenameIndex(
                name: "IX_HistoryAction_ActionByID",
                table: "HistoryActions",
                newName: "IX_HistoryActions_ActionByID");

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

            migrationBuilder.AddPrimaryKey(
                name: "PK_HistoryActions",
                table: "HistoryActions",
                column: "ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ActionBookings",
                table: "ActionBookings",
                column: "ID");

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

            migrationBuilder.CreateIndex(
                name: "IX_ImagePets_PetId",
                table: "ImagePets",
                column: "PetId");

            migrationBuilder.AddForeignKey(
                name: "FK_HistoryActions_ActionBookings_ActionID",
                table: "HistoryActions",
                column: "ActionID",
                principalTable: "ActionBookings",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_HistoryActions_AspNetUsers_ActionByID",
                table: "HistoryActions",
                column: "ActionByID",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_HistoryActions_Bookings_BookingID",
                table: "HistoryActions",
                column: "BookingID",
                principalTable: "Bookings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HistoryActions_ActionBookings_ActionID",
                table: "HistoryActions");

            migrationBuilder.DropForeignKey(
                name: "FK_HistoryActions_AspNetUsers_ActionByID",
                table: "HistoryActions");

            migrationBuilder.DropForeignKey(
                name: "FK_HistoryActions_Bookings_BookingID",
                table: "HistoryActions");

            migrationBuilder.DropTable(
                name: "ImagePets");

            migrationBuilder.DropPrimaryKey(
                name: "PK_HistoryActions",
                table: "HistoryActions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ActionBookings",
                table: "ActionBookings");

            migrationBuilder.DropColumn(
                name: "Desciption",
                table: "Services");

            migrationBuilder.DropColumn(
                name: "MaxWeight",
                table: "ServiceDetails");

            migrationBuilder.DropColumn(
                name: "MinWeight",
                table: "ServiceDetails");

            migrationBuilder.RenameTable(
                name: "HistoryActions",
                newName: "HistoryAction");

            migrationBuilder.RenameTable(
                name: "ActionBookings",
                newName: "ActionBooking");

            migrationBuilder.RenameIndex(
                name: "IX_HistoryActions_BookingID",
                table: "HistoryAction",
                newName: "IX_HistoryAction_BookingID");

            migrationBuilder.RenameIndex(
                name: "IX_HistoryActions_ActionID",
                table: "HistoryAction",
                newName: "IX_HistoryAction_ActionID");

            migrationBuilder.RenameIndex(
                name: "IX_HistoryActions_ActionByID",
                table: "HistoryAction",
                newName: "IX_HistoryAction_ActionByID");

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

            migrationBuilder.AddPrimaryKey(
                name: "PK_HistoryAction",
                table: "HistoryAction",
                column: "ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ActionBooking",
                table: "ActionBooking",
                column: "ID");

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
    }
}
