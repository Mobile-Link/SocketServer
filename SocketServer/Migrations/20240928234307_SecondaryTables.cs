using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SocketServer.Migrations
{
    /// <inheritdoc />
    public partial class SecondaryTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Devices_Users_UserIdUser",
                table: "Devices");

            migrationBuilder.DropForeignKey(
                name: "FK_Storages_Users_UserIdUser",
                table: "Storages");

            migrationBuilder.DropForeignKey(
                name: "FK_Transfers_Devices_DeviceDestinationIdDevice",
                table: "Transfers");

            migrationBuilder.DropForeignKey(
                name: "FK_Transfers_Devices_DeviceOriginIdDevice",
                table: "Transfers");

            migrationBuilder.DropForeignKey(
                name: "FK_Transfers_Users_UserIdUser",
                table: "Transfers");

            migrationBuilder.RenameColumn(
                name: "UserIdUser",
                table: "Transfers",
                newName: "idUser");

            migrationBuilder.RenameColumn(
                name: "DeviceOriginIdDevice",
                table: "Transfers",
                newName: "idDeviceOrigin");

            migrationBuilder.RenameColumn(
                name: "DeviceDestinationIdDevice",
                table: "Transfers",
                newName: "idDeviceDestination");

            migrationBuilder.RenameIndex(
                name: "IX_Transfers_UserIdUser",
                table: "Transfers",
                newName: "IX_Transfers_idUser");

            migrationBuilder.RenameIndex(
                name: "IX_Transfers_DeviceOriginIdDevice",
                table: "Transfers",
                newName: "IX_Transfers_idDeviceOrigin");

            migrationBuilder.RenameIndex(
                name: "IX_Transfers_DeviceDestinationIdDevice",
                table: "Transfers",
                newName: "IX_Transfers_idDeviceDestination");

            migrationBuilder.RenameColumn(
                name: "UserIdUser",
                table: "Storages",
                newName: "idUser");

            migrationBuilder.RenameIndex(
                name: "IX_Storages_UserIdUser",
                table: "Storages",
                newName: "IX_Storages_idUser");

            migrationBuilder.RenameColumn(
                name: "UserIdUser",
                table: "Devices",
                newName: "idUser");

            migrationBuilder.RenameIndex(
                name: "IX_Devices_UserIdUser",
                table: "Devices",
                newName: "IX_Devices_idUser");

            migrationBuilder.AddColumn<int>(
                name: "enDeviceOS",
                table: "Devices",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "EnDeviceOS",
                columns: table => new
                {
                    idDeviceOS = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    description = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EnDeviceOS", x => x.idDeviceOS);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Devices_enDeviceOS",
                table: "Devices",
                column: "enDeviceOS");

            migrationBuilder.AddForeignKey(
                name: "FK_Devices_EnDeviceOS_enDeviceOS",
                table: "Devices",
                column: "enDeviceOS",
                principalTable: "EnDeviceOS",
                principalColumn: "idDeviceOS",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Devices_Users_idUser",
                table: "Devices",
                column: "idUser",
                principalTable: "Users",
                principalColumn: "IdUser",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Storages_Users_idUser",
                table: "Storages",
                column: "idUser",
                principalTable: "Users",
                principalColumn: "IdUser",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Transfers_Devices_idDeviceDestination",
                table: "Transfers",
                column: "idDeviceDestination",
                principalTable: "Devices",
                principalColumn: "IdDevice",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Transfers_Devices_idDeviceOrigin",
                table: "Transfers",
                column: "idDeviceOrigin",
                principalTable: "Devices",
                principalColumn: "IdDevice",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Transfers_Users_idUser",
                table: "Transfers",
                column: "idUser",
                principalTable: "Users",
                principalColumn: "IdUser",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Devices_EnDeviceOS_enDeviceOS",
                table: "Devices");

            migrationBuilder.DropForeignKey(
                name: "FK_Devices_Users_idUser",
                table: "Devices");

            migrationBuilder.DropForeignKey(
                name: "FK_Storages_Users_idUser",
                table: "Storages");

            migrationBuilder.DropForeignKey(
                name: "FK_Transfers_Devices_idDeviceDestination",
                table: "Transfers");

            migrationBuilder.DropForeignKey(
                name: "FK_Transfers_Devices_idDeviceOrigin",
                table: "Transfers");

            migrationBuilder.DropForeignKey(
                name: "FK_Transfers_Users_idUser",
                table: "Transfers");

            migrationBuilder.DropTable(
                name: "EnDeviceOS");

            migrationBuilder.DropIndex(
                name: "IX_Devices_enDeviceOS",
                table: "Devices");

            migrationBuilder.DropColumn(
                name: "enDeviceOS",
                table: "Devices");

            migrationBuilder.RenameColumn(
                name: "idUser",
                table: "Transfers",
                newName: "UserIdUser");

            migrationBuilder.RenameColumn(
                name: "idDeviceOrigin",
                table: "Transfers",
                newName: "DeviceOriginIdDevice");

            migrationBuilder.RenameColumn(
                name: "idDeviceDestination",
                table: "Transfers",
                newName: "DeviceDestinationIdDevice");

            migrationBuilder.RenameIndex(
                name: "IX_Transfers_idUser",
                table: "Transfers",
                newName: "IX_Transfers_UserIdUser");

            migrationBuilder.RenameIndex(
                name: "IX_Transfers_idDeviceOrigin",
                table: "Transfers",
                newName: "IX_Transfers_DeviceOriginIdDevice");

            migrationBuilder.RenameIndex(
                name: "IX_Transfers_idDeviceDestination",
                table: "Transfers",
                newName: "IX_Transfers_DeviceDestinationIdDevice");

            migrationBuilder.RenameColumn(
                name: "idUser",
                table: "Storages",
                newName: "UserIdUser");

            migrationBuilder.RenameIndex(
                name: "IX_Storages_idUser",
                table: "Storages",
                newName: "IX_Storages_UserIdUser");

            migrationBuilder.RenameColumn(
                name: "idUser",
                table: "Devices",
                newName: "UserIdUser");

            migrationBuilder.RenameIndex(
                name: "IX_Devices_idUser",
                table: "Devices",
                newName: "IX_Devices_UserIdUser");

            migrationBuilder.AddForeignKey(
                name: "FK_Devices_Users_UserIdUser",
                table: "Devices",
                column: "UserIdUser",
                principalTable: "Users",
                principalColumn: "IdUser",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Storages_Users_UserIdUser",
                table: "Storages",
                column: "UserIdUser",
                principalTable: "Users",
                principalColumn: "IdUser",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Transfers_Devices_DeviceDestinationIdDevice",
                table: "Transfers",
                column: "DeviceDestinationIdDevice",
                principalTable: "Devices",
                principalColumn: "IdDevice",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Transfers_Devices_DeviceOriginIdDevice",
                table: "Transfers",
                column: "DeviceOriginIdDevice",
                principalTable: "Devices",
                principalColumn: "IdDevice",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Transfers_Users_UserIdUser",
                table: "Transfers",
                column: "UserIdUser",
                principalTable: "Users",
                principalColumn: "IdUser",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
