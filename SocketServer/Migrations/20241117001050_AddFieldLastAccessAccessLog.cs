using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SocketServer.Migrations
{
    /// <inheritdoc />
    public partial class AddFieldLastAccessAccessLog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "LastAccessDate",
                table: "Devices",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastAccessDate",
                table: "Devices");
        }
    }
}
