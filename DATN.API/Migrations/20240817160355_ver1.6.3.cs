using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DATN.API.Migrations
{
    /// <inheritdoc />
    public partial class ver163 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookingDetails_ComboServices_ComboId",
                table: "BookingDetails");

            migrationBuilder.DropTable(
                name: "ComboDetails");

            migrationBuilder.DropTable(
                name: "ComboServices");

            migrationBuilder.DropIndex(
                name: "IX_BookingDetails_ComboId",
                table: "BookingDetails");

            migrationBuilder.DropColumn(
                name: "CodeConfirm",
                table: "Guests");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CodeConfirm",
                table: "Guests",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ComboServices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PetTypeId = table.Column<int>(type: "int", nullable: false),
                    CreateAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeleteAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComboServices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ComboServices_PetTypes_PetTypeId",
                        column: x => x.PetTypeId,
                        principalTable: "PetTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ComboDetails",
                columns: table => new
                {
                    ServiceDetailId = table.Column<int>(type: "int", nullable: false),
                    ComboServiceId = table.Column<int>(type: "int", nullable: false),
                    ServiceId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComboDetails", x => new { x.ServiceDetailId, x.ComboServiceId });
                    table.ForeignKey(
                        name: "FK_ComboDetails_ComboServices_ComboServiceId",
                        column: x => x.ComboServiceId,
                        principalTable: "ComboServices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ComboDetails_ServiceDetails_ServiceDetailId",
                        column: x => x.ServiceDetailId,
                        principalTable: "ServiceDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ComboDetails_Services_ServiceId",
                        column: x => x.ServiceId,
                        principalTable: "Services",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_BookingDetails_ComboId",
                table: "BookingDetails",
                column: "ComboId");

            migrationBuilder.CreateIndex(
                name: "IX_ComboDetails_ComboServiceId",
                table: "ComboDetails",
                column: "ComboServiceId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ComboDetails_ServiceId",
                table: "ComboDetails",
                column: "ServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_ComboServices_PetTypeId",
                table: "ComboServices",
                column: "PetTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_BookingDetails_ComboServices_ComboId",
                table: "BookingDetails",
                column: "ComboId",
                principalTable: "ComboServices",
                principalColumn: "Id");
        }
    }
}
