using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SocketServer.Migrations
{
    /// <inheritdoc />
    public partial class CorrectionEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Devices_EnDeviceOSs_enDeviceOS",
                table: "Devices");

            migrationBuilder.DropForeignKey(
                name: "FK_History_EnActions_enAction",
                table: "History");

            migrationBuilder.DropForeignKey(
                name: "FK_TransferenceLogs_EnStatuses_enStatus",
                table: "TransferenceLogs");

            migrationBuilder.DropTable(
                name: "VerificationCodes");

            migrationBuilder.DropIndex(
                name: "IX_TransferenceLogs_enStatus",
                table: "TransferenceLogs");

            migrationBuilder.DropIndex(
                name: "IX_History_enAction",
                table: "History");

            migrationBuilder.DropIndex(
                name: "IX_Devices_enDeviceOS",
                table: "Devices");

            migrationBuilder.RenameColumn(
                name: "enStatus",
                table: "TransferenceLogs",
                newName: "EnStatusType");

            migrationBuilder.RenameColumn(
                name: "enAction",
                table: "History",
                newName: "EnActionType");

            migrationBuilder.RenameColumn(
                name: "enDeviceOS",
                table: "Devices",
                newName: "EnDeviceOsType");

            migrationBuilder.AddColumn<DateTime>(
                name: "Date",
                table: "TransferenceLogs",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "ServePath",
                table: "TransferenceLogs",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "Date",
                table: "History",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "History",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Date",
                table: "TransferenceLogs");

            migrationBuilder.DropColumn(
                name: "ServePath",
                table: "TransferenceLogs");

            migrationBuilder.DropColumn(
                name: "Date",
                table: "History");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "History");

            migrationBuilder.RenameColumn(
                name: "EnStatusType",
                table: "TransferenceLogs",
                newName: "enStatus");

            migrationBuilder.RenameColumn(
                name: "EnActionType",
                table: "History",
                newName: "enAction");

            migrationBuilder.RenameColumn(
                name: "EnDeviceOsType",
                table: "Devices",
                newName: "enDeviceOS");

            migrationBuilder.CreateTable(
                name: "VerificationCodes",
                columns: table => new
                {
                    IdCode = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Code = table.Column<string>(type: "TEXT", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Email = table.Column<string>(type: "TEXT", nullable: false),
                    ExpirationDate = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VerificationCodes", x => x.IdCode);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TransferenceLogs_enStatus",
                table: "TransferenceLogs",
                column: "enStatus");

            migrationBuilder.CreateIndex(
                name: "IX_History_enAction",
                table: "History",
                column: "enAction");

            migrationBuilder.CreateIndex(
                name: "IX_Devices_enDeviceOS",
                table: "Devices",
                column: "enDeviceOS");

            migrationBuilder.AddForeignKey(
                name: "FK_Devices_EnDeviceOSs_enDeviceOS",
                table: "Devices",
                column: "enDeviceOS",
                principalTable: "EnDeviceOSs",
                principalColumn: "idDeviceOS",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_History_EnActions_enAction",
                table: "History",
                column: "enAction",
                principalTable: "EnActions",
                principalColumn: "idAction",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TransferenceLogs_EnStatuses_enStatus",
                table: "TransferenceLogs",
                column: "enStatus",
                principalTable: "EnStatuses",
                principalColumn: "idStatus",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
