using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SocketServer.Migrations
{
    /// <inheritdoc />
    public partial class RemoveDeviceToken : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_History_Devices_idDevice",
                table: "History");

            migrationBuilder.DropForeignKey(
                name: "FK_History_Users_idUser",
                table: "History");

            migrationBuilder.DropPrimaryKey(
                name: "PK_History",
                table: "History");

            migrationBuilder.DropColumn(
                name: "Token",
                table: "Devices");

            migrationBuilder.RenameTable(
                name: "History",
                newName: "Histories");

            migrationBuilder.RenameIndex(
                name: "IX_History_idUser",
                table: "Histories",
                newName: "IX_Histories_idUser");

            migrationBuilder.RenameIndex(
                name: "IX_History_idDevice",
                table: "Histories",
                newName: "IX_Histories_idDevice");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Histories",
                table: "Histories",
                column: "idHistory");

            migrationBuilder.AddForeignKey(
                name: "FK_Histories_Devices_idDevice",
                table: "Histories",
                column: "idDevice",
                principalTable: "Devices",
                principalColumn: "IdDevice",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Histories_Users_idUser",
                table: "Histories",
                column: "idUser",
                principalTable: "Users",
                principalColumn: "IdUser",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Histories_Devices_idDevice",
                table: "Histories");

            migrationBuilder.DropForeignKey(
                name: "FK_Histories_Users_idUser",
                table: "Histories");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Histories",
                table: "Histories");

            migrationBuilder.RenameTable(
                name: "Histories",
                newName: "History");

            migrationBuilder.RenameIndex(
                name: "IX_Histories_idUser",
                table: "History",
                newName: "IX_History_idUser");

            migrationBuilder.RenameIndex(
                name: "IX_Histories_idDevice",
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
        }
    }
}
