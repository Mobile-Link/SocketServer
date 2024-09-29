using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SocketServer.Migrations
{
    /// <inheritdoc />
    public partial class AllTablesMap : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Devices_EnDeviceOS_enDeviceOS",
                table: "Devices");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EnDeviceOS",
                table: "EnDeviceOS");

            migrationBuilder.RenameTable(
                name: "EnDeviceOS",
                newName: "EnDeviceOSs");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EnDeviceOSs",
                table: "EnDeviceOSs",
                column: "idDeviceOS");

            migrationBuilder.CreateTable(
                name: "AccessLogs",
                columns: table => new
                {
                    idAccessLog = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    idUser = table.Column<int>(type: "INTEGER", nullable: false),
                    idDevice = table.Column<int>(type: "INTEGER", nullable: false),
                    Date = table.Column<DateTime>(type: "TEXT", nullable: false),
                    AccessLocation = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccessLogs", x => x.idAccessLog);
                    table.ForeignKey(
                        name: "FK_AccessLogs_Devices_idDevice",
                        column: x => x.idDevice,
                        principalTable: "Devices",
                        principalColumn: "IdDevice",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AccessLogs_Users_idUser",
                        column: x => x.idUser,
                        principalTable: "Users",
                        principalColumn: "IdUser",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EnActions",
                columns: table => new
                {
                    idAction = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    description = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EnActions", x => x.idAction);
                });

            migrationBuilder.CreateTable(
                name: "EnStatuses",
                columns: table => new
                {
                    idStatus = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    description = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EnStatuses", x => x.idStatus);
                });

            migrationBuilder.CreateTable(
                name: "History",
                columns: table => new
                {
                    idHistory = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    idUser = table.Column<int>(type: "INTEGER", nullable: false),
                    idDevice = table.Column<int>(type: "INTEGER", nullable: false),
                    enAction = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_History", x => x.idHistory);
                    table.ForeignKey(
                        name: "FK_History_Devices_idDevice",
                        column: x => x.idDevice,
                        principalTable: "Devices",
                        principalColumn: "IdDevice",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_History_EnActions_enAction",
                        column: x => x.enAction,
                        principalTable: "EnActions",
                        principalColumn: "idAction",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_History_Users_idUser",
                        column: x => x.idUser,
                        principalTable: "Users",
                        principalColumn: "IdUser",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TransferenceLogs",
                columns: table => new
                {
                    idTransferenceLog = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    idTransference = table.Column<int>(type: "INTEGER", nullable: false),
                    enStatus = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransferenceLogs", x => x.idTransferenceLog);
                    table.ForeignKey(
                        name: "FK_TransferenceLogs_EnStatuses_enStatus",
                        column: x => x.enStatus,
                        principalTable: "EnStatuses",
                        principalColumn: "idStatus",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TransferenceLogs_Transfers_idTransference",
                        column: x => x.idTransference,
                        principalTable: "Transfers",
                        principalColumn: "idTranference",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AccessLogs_idDevice",
                table: "AccessLogs",
                column: "idDevice");

            migrationBuilder.CreateIndex(
                name: "IX_AccessLogs_idUser",
                table: "AccessLogs",
                column: "idUser");

            migrationBuilder.CreateIndex(
                name: "IX_History_enAction",
                table: "History",
                column: "enAction");

            migrationBuilder.CreateIndex(
                name: "IX_History_idDevice",
                table: "History",
                column: "idDevice");

            migrationBuilder.CreateIndex(
                name: "IX_History_idUser",
                table: "History",
                column: "idUser");

            migrationBuilder.CreateIndex(
                name: "IX_TransferenceLogs_enStatus",
                table: "TransferenceLogs",
                column: "enStatus");

            migrationBuilder.CreateIndex(
                name: "IX_TransferenceLogs_idTransference",
                table: "TransferenceLogs",
                column: "idTransference");

            migrationBuilder.AddForeignKey(
                name: "FK_Devices_EnDeviceOSs_enDeviceOS",
                table: "Devices",
                column: "enDeviceOS",
                principalTable: "EnDeviceOSs",
                principalColumn: "idDeviceOS",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Devices_EnDeviceOSs_enDeviceOS",
                table: "Devices");

            migrationBuilder.DropTable(
                name: "AccessLogs");

            migrationBuilder.DropTable(
                name: "History");

            migrationBuilder.DropTable(
                name: "TransferenceLogs");

            migrationBuilder.DropTable(
                name: "EnActions");

            migrationBuilder.DropTable(
                name: "EnStatuses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EnDeviceOSs",
                table: "EnDeviceOSs");

            migrationBuilder.RenameTable(
                name: "EnDeviceOSs",
                newName: "EnDeviceOS");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EnDeviceOS",
                table: "EnDeviceOS",
                column: "idDeviceOS");

            migrationBuilder.AddForeignKey(
                name: "FK_Devices_EnDeviceOS_enDeviceOS",
                table: "Devices",
                column: "enDeviceOS",
                principalTable: "EnDeviceOS",
                principalColumn: "idDeviceOS",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
