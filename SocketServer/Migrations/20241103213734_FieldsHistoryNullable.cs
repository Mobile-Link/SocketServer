using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SocketServer.Migrations
{
    /// <inheritdoc />
    public partial class FieldsHistoryNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Histories_Devices_IdDevice",
                table: "Histories");

            migrationBuilder.AlterColumn<int>(
                name: "IdDevice",
                table: "Histories",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddForeignKey(
                name: "FK_Histories_Devices_IdDevice",
                table: "Histories",
                column: "IdDevice",
                principalTable: "Devices",
                principalColumn: "IdDevice");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Histories_Devices_IdDevice",
                table: "Histories");

            migrationBuilder.AlterColumn<int>(
                name: "IdDevice",
                table: "Histories",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Histories_Devices_IdDevice",
                table: "Histories",
                column: "IdDevice",
                principalTable: "Devices",
                principalColumn: "IdDevice",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
