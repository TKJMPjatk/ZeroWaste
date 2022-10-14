using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ZeroWaste.Migrations
{
    public partial class photoFix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PhotoBinary",
                table: "Photos");

            migrationBuilder.AddColumn<byte[]>(
                name: "BinaryPhoto",
                table: "Photos",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0]);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BinaryPhoto",
                table: "Photos");

            migrationBuilder.AddColumn<string>(
                name: "PhotoBinary",
                table: "Photos",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
