using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SocketServer.Migrations
{
    /// <inheritdoc />
    public partial class RemoveFieldsHistory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Histories_EnActions_IdAction",
                table: "Histories");

            migrationBuilder.DropIndex(
                name: "IX_Histories_IdAction",
                table: "Histories");

            migrationBuilder.DropColumn(
                name: "IdAction",
                table: "Histories");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IdAction",
                table: "Histories",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Histories_IdAction",
                table: "Histories",
                column: "IdAction");

            migrationBuilder.AddForeignKey(
                name: "FK_Histories_EnActions_IdAction",
                table: "Histories",
                column: "IdAction",
                principalTable: "EnActions",
                principalColumn: "IdAction",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
