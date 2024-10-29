using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SocketServer.Migrations
{
    /// <inheritdoc />
    public partial class TransferenceChunk : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TransferenceLogs");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreationDate",
                table: "Transfers",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "EnStatus",
                table: "Transfers",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdateDate",
                table: "Transfers",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateTable(
                name: "EnChunkStatuses",
                columns: table => new
                {
                    IdChunkStatus = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Description = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EnChunkStatuses", x => x.IdChunkStatus);
                });

            migrationBuilder.CreateTable(
                name: "TransferenceChunks",
                columns: table => new
                {
                    IdTranferenceChunck = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IdTransference = table.Column<int>(type: "INTEGER", nullable: false),
                    startByteIndex = table.Column<long>(type: "INTEGER", nullable: false),
                    EnChunkStatus = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransferenceChunks", x => x.IdTranferenceChunck);
                    table.ForeignKey(
                        name: "FK_TransferenceChunks_Transfers_IdTransference",
                        column: x => x.IdTransference,
                        principalTable: "Transfers",
                        principalColumn: "IdTranference",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TransferenceChunks_IdTransference",
                table: "TransferenceChunks",
                column: "IdTransference");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EnChunkStatuses");

            migrationBuilder.DropTable(
                name: "TransferenceChunks");

            migrationBuilder.DropColumn(
                name: "CreationDate",
                table: "Transfers");

            migrationBuilder.DropColumn(
                name: "EnStatus",
                table: "Transfers");

            migrationBuilder.DropColumn(
                name: "UpdateDate",
                table: "Transfers");

            migrationBuilder.CreateTable(
                name: "TransferenceLogs",
                columns: table => new
                {
                    IdTransferenceLog = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IdTransference = table.Column<int>(type: "INTEGER", nullable: false),
                    Date = table.Column<DateTime>(type: "TEXT", nullable: false),
                    EnStatus = table.Column<int>(type: "INTEGER", nullable: false),
                    ServePath = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransferenceLogs", x => x.IdTransferenceLog);
                    table.ForeignKey(
                        name: "FK_TransferenceLogs_Transfers_IdTransference",
                        column: x => x.IdTransference,
                        principalTable: "Transfers",
                        principalColumn: "IdTranference",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TransferenceLogs_IdTransference",
                table: "TransferenceLogs",
                column: "IdTransference");
        }
    }
}
