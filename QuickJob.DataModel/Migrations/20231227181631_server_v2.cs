using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuickJob.DataModel.Migrations
{
    /// <inheritdoc />
    public partial class server_v2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreateDateTime",
                table: "responses",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "EditDateTime",
                table: "responses",
                type: "timestamp with time zone",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreateDateTime",
                table: "responses");

            migrationBuilder.DropColumn(
                name: "EditDateTime",
                table: "responses");
        }
    }
}
