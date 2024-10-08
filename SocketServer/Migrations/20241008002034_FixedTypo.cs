using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SocketServer.Migrations
{
    /// <inheritdoc />
    public partial class FixedTypo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Histories_Users_FkIdUser",
                table: "Histories");

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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Histories_Users_IdUser",
                table: "Histories");

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
        }
    }
}
