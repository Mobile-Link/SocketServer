using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SocketServer.Migrations
{
    /// <inheritdoc />
    public partial class RenameEnums : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "EnStatusType",
                table: "TransferenceLogs",
                newName: "EnStatus");

            migrationBuilder.RenameColumn(
                name: "EnActionType",
                table: "Histories",
                newName: "EnAction");

            migrationBuilder.RenameColumn(
                name: "EnDeviceOsType",
                table: "Devices",
                newName: "EnDeviceOs");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "EnStatus",
                table: "TransferenceLogs",
                newName: "EnStatusType");

            migrationBuilder.RenameColumn(
                name: "EnAction",
                table: "Histories",
                newName: "EnActionType");

            migrationBuilder.RenameColumn(
                name: "EnDeviceOs",
                table: "Devices",
                newName: "EnDeviceOsType");
        }
    }
}
