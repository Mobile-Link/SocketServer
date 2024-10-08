using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SocketServer.Migrations
{
    /// <inheritdoc />
    public partial class RenameStorageFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Histories_Users_IdUser",
                table: "Histories");

            migrationBuilder.DropForeignKey(
                name: "FK_Storages_Users_idUser",
                table: "Storages");

            migrationBuilder.RenameColumn(
                name: "idUser",
                table: "Storages",
                newName: "IdUser");

            migrationBuilder.RenameColumn(
                name: "IdDevice",
                table: "Storages",
                newName: "IdStorage");

            migrationBuilder.RenameIndex(
                name: "IX_Storages_idUser",
                table: "Storages",
                newName: "IX_Storages_IdUser");

            migrationBuilder.RenameColumn(
                name: "IdUser",
                table: "Histories",
                newName: "FkIdUser");

            migrationBuilder.RenameIndex(
                name: "IX_Histories_IdUser",
                table: "Histories",
                newName: "IX_Histories_FkIdUser");

            migrationBuilder.AddForeignKey(
                name: "FK_Histories_Users_FkIdUser",
                table: "Histories",
                column: "FkIdUser",
                principalTable: "Users",
                principalColumn: "IdUser",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Storages_Users_IdUser",
                table: "Storages",
                column: "IdUser",
                principalTable: "Users",
                principalColumn: "IdUser",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Histories_Users_FkIdUser",
                table: "Histories");

            migrationBuilder.DropForeignKey(
                name: "FK_Storages_Users_IdUser",
                table: "Storages");

            migrationBuilder.RenameColumn(
                name: "IdUser",
                table: "Storages",
                newName: "idUser");

            migrationBuilder.RenameColumn(
                name: "IdStorage",
                table: "Storages",
                newName: "IdDevice");

            migrationBuilder.RenameIndex(
                name: "IX_Storages_IdUser",
                table: "Storages",
                newName: "IX_Storages_idUser");

            migrationBuilder.RenameColumn(
                name: "FkIdUser",
                table: "Histories",
                newName: "IdUser");

            migrationBuilder.RenameIndex(
                name: "IX_Histories_FkIdUser",
                table: "Histories",
                newName: "IX_Histories_IdUser");

            migrationBuilder.AddForeignKey(
                name: "FK_Histories_Users_IdUser",
                table: "Histories",
                column: "IdUser",
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
        }
    }
}
