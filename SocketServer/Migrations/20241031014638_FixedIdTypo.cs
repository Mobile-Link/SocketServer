using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SocketServer.Migrations
{
    /// <inheritdoc />
    public partial class FixedIdTypo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IdTranference",
                table: "Transfers",
                newName: "IdTransference");

            migrationBuilder.RenameColumn(
                name: "IdTranferenceChunck",
                table: "TransferenceChunks",
                newName: "IdTransferenceChunk");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IdTransference",
                table: "Transfers",
                newName: "IdTranference");

            migrationBuilder.RenameColumn(
                name: "IdTransferenceChunk",
                table: "TransferenceChunks",
                newName: "IdTranferenceChunck");
        }
    }
}
