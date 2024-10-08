using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SocketServer.Migrations
{
    /// <inheritdoc />
    public partial class RenameCamelCaseFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AccessLogs_Devices_idDevice",
                table: "AccessLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_AccessLogs_Users_idUser",
                table: "AccessLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_Devices_Users_idUser",
                table: "Devices");

            migrationBuilder.DropForeignKey(
                name: "FK_History_Devices_idDevice",
                table: "History");

            migrationBuilder.DropForeignKey(
                name: "FK_History_Users_idUser",
                table: "History");

            migrationBuilder.DropForeignKey(
                name: "FK_TransferenceLogs_Transfers_idTransference",
                table: "TransferenceLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_Transfers_Devices_idDeviceDestination",
                table: "Transfers");

            migrationBuilder.DropForeignKey(
                name: "FK_Transfers_Devices_idDeviceOrigin",
                table: "Transfers");

            migrationBuilder.DropForeignKey(
                name: "FK_Transfers_Users_idUser",
                table: "Transfers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_History",
                table: "History");

            migrationBuilder.DropColumn(
                name: "Token",
                table: "Devices");

            migrationBuilder.RenameTable(
                name: "History",
                newName: "Histories");

            migrationBuilder.RenameColumn(
                name: "idUser",
                table: "Transfers",
                newName: "IdUser");

            migrationBuilder.RenameColumn(
                name: "idDeviceOrigin",
                table: "Transfers",
                newName: "IdDeviceOrigin");

            migrationBuilder.RenameColumn(
                name: "idDeviceDestination",
                table: "Transfers",
                newName: "IdDeviceDestination");

            migrationBuilder.RenameColumn(
                name: "idTranference",
                table: "Transfers",
                newName: "IdTranference");

            migrationBuilder.RenameIndex(
                name: "IX_Transfers_idUser",
                table: "Transfers",
                newName: "IX_Transfers_IdUser");

            migrationBuilder.RenameIndex(
                name: "IX_Transfers_idDeviceOrigin",
                table: "Transfers",
                newName: "IX_Transfers_IdDeviceOrigin");

            migrationBuilder.RenameIndex(
                name: "IX_Transfers_idDeviceDestination",
                table: "Transfers",
                newName: "IX_Transfers_IdDeviceDestination");

            migrationBuilder.RenameColumn(
                name: "idTransference",
                table: "TransferenceLogs",
                newName: "IdTransference");

            migrationBuilder.RenameColumn(
                name: "idTransferenceLog",
                table: "TransferenceLogs",
                newName: "IdTransferenceLog");

            migrationBuilder.RenameIndex(
                name: "IX_TransferenceLogs_idTransference",
                table: "TransferenceLogs",
                newName: "IX_TransferenceLogs_IdTransference");

            migrationBuilder.RenameColumn(
                name: "description",
                table: "EnStatuses",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "idStatus",
                table: "EnStatuses",
                newName: "IdStatus");

            migrationBuilder.RenameColumn(
                name: "description",
                table: "EnDeviceOSs",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "idDeviceOS",
                table: "EnDeviceOSs",
                newName: "IdDeviceOs");

            migrationBuilder.RenameColumn(
                name: "description",
                table: "EnActions",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "idAction",
                table: "EnActions",
                newName: "IdAction");

            migrationBuilder.RenameColumn(
                name: "idUser",
                table: "Devices",
                newName: "IdUser");

            migrationBuilder.RenameIndex(
                name: "IX_Devices_idUser",
                table: "Devices",
                newName: "IX_Devices_IdUser");

            migrationBuilder.RenameColumn(
                name: "idUser",
                table: "AccessLogs",
                newName: "IdUser");

            migrationBuilder.RenameColumn(
                name: "idDevice",
                table: "AccessLogs",
                newName: "IdDevice");

            migrationBuilder.RenameColumn(
                name: "idAccessLog",
                table: "AccessLogs",
                newName: "IdAccessLog");

            migrationBuilder.RenameIndex(
                name: "IX_AccessLogs_idUser",
                table: "AccessLogs",
                newName: "IX_AccessLogs_IdUser");

            migrationBuilder.RenameIndex(
                name: "IX_AccessLogs_idDevice",
                table: "AccessLogs",
                newName: "IX_AccessLogs_IdDevice");

            migrationBuilder.RenameColumn(
                name: "idUser",
                table: "Histories",
                newName: "IdUser");

            migrationBuilder.RenameColumn(
                name: "idDevice",
                table: "Histories",
                newName: "IdDevice");

            migrationBuilder.RenameColumn(
                name: "idHistory",
                table: "Histories",
                newName: "IdHistory");

            migrationBuilder.RenameIndex(
                name: "IX_History_idUser",
                table: "Histories",
                newName: "IX_Histories_IdUser");

            migrationBuilder.RenameIndex(
                name: "IX_History_idDevice",
                table: "Histories",
                newName: "IX_Histories_IdDevice");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Histories",
                table: "Histories",
                column: "IdHistory");

            migrationBuilder.AddForeignKey(
                name: "FK_AccessLogs_Devices_IdDevice",
                table: "AccessLogs",
                column: "IdDevice",
                principalTable: "Devices",
                principalColumn: "IdDevice",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AccessLogs_Users_IdUser",
                table: "AccessLogs",
                column: "IdUser",
                principalTable: "Users",
                principalColumn: "IdUser",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Devices_Users_IdUser",
                table: "Devices",
                column: "IdUser",
                principalTable: "Users",
                principalColumn: "IdUser",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Histories_Devices_IdDevice",
                table: "Histories",
                column: "IdDevice",
                principalTable: "Devices",
                principalColumn: "IdDevice",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Histories_Users_IdUser",
                table: "Histories",
                column: "IdUser",
                principalTable: "Users",
                principalColumn: "IdUser",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TransferenceLogs_Transfers_IdTransference",
                table: "TransferenceLogs",
                column: "IdTransference",
                principalTable: "Transfers",
                principalColumn: "IdTranference",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Transfers_Devices_IdDeviceDestination",
                table: "Transfers",
                column: "IdDeviceDestination",
                principalTable: "Devices",
                principalColumn: "IdDevice",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Transfers_Devices_IdDeviceOrigin",
                table: "Transfers",
                column: "IdDeviceOrigin",
                principalTable: "Devices",
                principalColumn: "IdDevice",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Transfers_Users_IdUser",
                table: "Transfers",
                column: "IdUser",
                principalTable: "Users",
                principalColumn: "IdUser",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AccessLogs_Devices_IdDevice",
                table: "AccessLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_AccessLogs_Users_IdUser",
                table: "AccessLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_Devices_Users_IdUser",
                table: "Devices");

            migrationBuilder.DropForeignKey(
                name: "FK_Histories_Devices_IdDevice",
                table: "Histories");

            migrationBuilder.DropForeignKey(
                name: "FK_Histories_Users_IdUser",
                table: "Histories");

            migrationBuilder.DropForeignKey(
                name: "FK_TransferenceLogs_Transfers_IdTransference",
                table: "TransferenceLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_Transfers_Devices_IdDeviceDestination",
                table: "Transfers");

            migrationBuilder.DropForeignKey(
                name: "FK_Transfers_Devices_IdDeviceOrigin",
                table: "Transfers");

            migrationBuilder.DropForeignKey(
                name: "FK_Transfers_Users_IdUser",
                table: "Transfers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Histories",
                table: "Histories");

            migrationBuilder.RenameTable(
                name: "Histories",
                newName: "History");

            migrationBuilder.RenameColumn(
                name: "IdUser",
                table: "Transfers",
                newName: "idUser");

            migrationBuilder.RenameColumn(
                name: "IdDeviceOrigin",
                table: "Transfers",
                newName: "idDeviceOrigin");

            migrationBuilder.RenameColumn(
                name: "IdDeviceDestination",
                table: "Transfers",
                newName: "idDeviceDestination");

            migrationBuilder.RenameColumn(
                name: "IdTranference",
                table: "Transfers",
                newName: "idTranference");

            migrationBuilder.RenameIndex(
                name: "IX_Transfers_IdUser",
                table: "Transfers",
                newName: "IX_Transfers_idUser");

            migrationBuilder.RenameIndex(
                name: "IX_Transfers_IdDeviceOrigin",
                table: "Transfers",
                newName: "IX_Transfers_idDeviceOrigin");

            migrationBuilder.RenameIndex(
                name: "IX_Transfers_IdDeviceDestination",
                table: "Transfers",
                newName: "IX_Transfers_idDeviceDestination");

            migrationBuilder.RenameColumn(
                name: "IdTransference",
                table: "TransferenceLogs",
                newName: "idTransference");

            migrationBuilder.RenameColumn(
                name: "IdTransferenceLog",
                table: "TransferenceLogs",
                newName: "idTransferenceLog");

            migrationBuilder.RenameIndex(
                name: "IX_TransferenceLogs_IdTransference",
                table: "TransferenceLogs",
                newName: "IX_TransferenceLogs_idTransference");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "EnStatuses",
                newName: "description");

            migrationBuilder.RenameColumn(
                name: "IdStatus",
                table: "EnStatuses",
                newName: "idStatus");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "EnDeviceOSs",
                newName: "description");

            migrationBuilder.RenameColumn(
                name: "IdDeviceOs",
                table: "EnDeviceOSs",
                newName: "idDeviceOS");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "EnActions",
                newName: "description");

            migrationBuilder.RenameColumn(
                name: "IdAction",
                table: "EnActions",
                newName: "idAction");

            migrationBuilder.RenameColumn(
                name: "IdUser",
                table: "Devices",
                newName: "idUser");

            migrationBuilder.RenameIndex(
                name: "IX_Devices_IdUser",
                table: "Devices",
                newName: "IX_Devices_idUser");

            migrationBuilder.RenameColumn(
                name: "IdUser",
                table: "AccessLogs",
                newName: "idUser");

            migrationBuilder.RenameColumn(
                name: "IdDevice",
                table: "AccessLogs",
                newName: "idDevice");

            migrationBuilder.RenameColumn(
                name: "IdAccessLog",
                table: "AccessLogs",
                newName: "idAccessLog");

            migrationBuilder.RenameIndex(
                name: "IX_AccessLogs_IdUser",
                table: "AccessLogs",
                newName: "IX_AccessLogs_idUser");

            migrationBuilder.RenameIndex(
                name: "IX_AccessLogs_IdDevice",
                table: "AccessLogs",
                newName: "IX_AccessLogs_idDevice");

            migrationBuilder.RenameColumn(
                name: "IdUser",
                table: "History",
                newName: "idUser");

            migrationBuilder.RenameColumn(
                name: "IdDevice",
                table: "History",
                newName: "idDevice");

            migrationBuilder.RenameColumn(
                name: "IdHistory",
                table: "History",
                newName: "idHistory");

            migrationBuilder.RenameIndex(
                name: "IX_Histories_IdUser",
                table: "History",
                newName: "IX_History_idUser");

            migrationBuilder.RenameIndex(
                name: "IX_Histories_IdDevice",
                table: "History",
                newName: "IX_History_idDevice");

            migrationBuilder.AddColumn<string>(
                name: "Token",
                table: "Devices",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_History",
                table: "History",
                column: "idHistory");

            migrationBuilder.AddForeignKey(
                name: "FK_AccessLogs_Devices_idDevice",
                table: "AccessLogs",
                column: "idDevice",
                principalTable: "Devices",
                principalColumn: "IdDevice",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AccessLogs_Users_idUser",
                table: "AccessLogs",
                column: "idUser",
                principalTable: "Users",
                principalColumn: "IdUser",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Devices_Users_idUser",
                table: "Devices",
                column: "idUser",
                principalTable: "Users",
                principalColumn: "IdUser",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_History_Devices_idDevice",
                table: "History",
                column: "idDevice",
                principalTable: "Devices",
                principalColumn: "IdDevice",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_History_Users_idUser",
                table: "History",
                column: "idUser",
                principalTable: "Users",
                principalColumn: "IdUser",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TransferenceLogs_Transfers_idTransference",
                table: "TransferenceLogs",
                column: "idTransference",
                principalTable: "Transfers",
                principalColumn: "idTranference",
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
    }
}
