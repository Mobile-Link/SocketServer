using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SocketServer.Migrations
{
    /// <inheritdoc />
    public partial class InitialTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Devices",
                columns: table => new
                {
                    IdDevice = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserIdUser = table.Column<int>(type: "INTEGER", nullable: false),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    LastLocation = table.Column<string>(type: "TEXT", nullable: false),
                    AvailableSpace = table.Column<long>(type: "INTEGER", nullable: false),
                    OccupiedSpace = table.Column<long>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Token = table.Column<string>(type: "TEXT", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    AlterationDate = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Devices", x => x.IdDevice);
                    table.ForeignKey(
                        name: "FK_Devices_Users_UserIdUser",
                        column: x => x.UserIdUser,
                        principalTable: "Users",
                        principalColumn: "IdUser",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Storages",
                columns: table => new
                {
                    IdDevice = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserIdUser = table.Column<int>(type: "INTEGER", nullable: false),
                    StorageLimitBytes = table.Column<long>(type: "INTEGER", nullable: false),
                    UsedStorageBytes = table.Column<long>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Storages", x => x.IdDevice);
                    table.ForeignKey(
                        name: "FK_Storages_Users_UserIdUser",
                        column: x => x.UserIdUser,
                        principalTable: "Users",
                        principalColumn: "IdUser",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Transfers",
                columns: table => new
                {
                    idTranference = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserIdUser = table.Column<int>(type: "INTEGER", nullable: false),
                    DeviceOriginIdDevice = table.Column<int>(type: "INTEGER", nullable: false),
                    DeviceDestinationIdDevice = table.Column<int>(type: "INTEGER", nullable: false),
                    FileExtention = table.Column<string>(type: "TEXT", nullable: false),
                    FileName = table.Column<string>(type: "TEXT", nullable: false),
                    Size = table.Column<long>(type: "INTEGER", nullable: false),
                    DestinationPath = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transfers", x => x.idTranference);
                    table.ForeignKey(
                        name: "FK_Transfers_Devices_DeviceDestinationIdDevice",
                        column: x => x.DeviceDestinationIdDevice,
                        principalTable: "Devices",
                        principalColumn: "IdDevice",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Transfers_Devices_DeviceOriginIdDevice",
                        column: x => x.DeviceOriginIdDevice,
                        principalTable: "Devices",
                        principalColumn: "IdDevice",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Transfers_Users_UserIdUser",
                        column: x => x.UserIdUser,
                        principalTable: "Users",
                        principalColumn: "IdUser",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Devices_UserIdUser",
                table: "Devices",
                column: "UserIdUser");

            migrationBuilder.CreateIndex(
                name: "IX_Storages_UserIdUser",
                table: "Storages",
                column: "UserIdUser");

            migrationBuilder.CreateIndex(
                name: "IX_Transfers_DeviceDestinationIdDevice",
                table: "Transfers",
                column: "DeviceDestinationIdDevice");

            migrationBuilder.CreateIndex(
                name: "IX_Transfers_DeviceOriginIdDevice",
                table: "Transfers",
                column: "DeviceOriginIdDevice");

            migrationBuilder.CreateIndex(
                name: "IX_Transfers_UserIdUser",
                table: "Transfers",
                column: "UserIdUser");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Storages");

            migrationBuilder.DropTable(
                name: "Transfers");

            migrationBuilder.DropTable(
                name: "Devices");
        }
    }
}
