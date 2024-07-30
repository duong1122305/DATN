using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DATN.API.Migrations
{
    /// <inheritdoc />
    public partial class no52 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_AspNetUsers_UserId",
                table: "Bookings");

            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_AspNetUsers_UserId1",
                table: "Bookings");

            migrationBuilder.DropForeignKey(
                name: "FK_HistoryAction_AspNetUsers_ActionByID",
                table: "HistoryAction");

            migrationBuilder.DropIndex(
                name: "IX_Bookings_UserId",
                table: "Bookings");

            migrationBuilder.DropIndex(
                name: "IX_Bookings_UserId1",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "Bookings");

            migrationBuilder.RenameColumn(
                name: "ActionByID",
                table: "HistoryAction",
                newName: "ActionById");

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

            migrationBuilder.AddColumn<Guid>(
                name: "ActionByStaff",
                table: "HistoryAction",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_HistoryAction_AspNetUsers_ActionById",
                table: "HistoryAction",
                column: "ActionById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HistoryAction_AspNetUsers_ActionById",
                table: "HistoryAction");

            migrationBuilder.DropColumn(
                name: "ActionByStaff",
                table: "HistoryAction");

            migrationBuilder.RenameColumn(
                name: "ActionById",
                table: "HistoryAction",
                newName: "ActionByID");

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

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "Bookings",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "UserId1",
                table: "Bookings",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_UserId",
                table: "Bookings",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_UserId1",
                table: "Bookings",
                column: "UserId1");

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

            migrationBuilder.AddForeignKey(
                name: "FK_HistoryAction_AspNetUsers_ActionByID",
                table: "HistoryAction",
                column: "ActionByID",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
