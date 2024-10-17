using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SocketServer.Migrations
{
    /// <inheritdoc />
    public partial class RemoveFieldTransference : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FileExtention",
                table: "Transfers");

            migrationBuilder.RenameColumn(
                name: "FileName",
                table: "Transfers",
                newName: "FilePath");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FilePath",
                table: "Transfers",
                newName: "FileName");

            migrationBuilder.AddColumn<string>(
                name: "FileExtention",
                table: "Transfers",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }
    }
}
