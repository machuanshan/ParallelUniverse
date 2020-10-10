using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ParallelUniverse.RazorPages.Migrations
{
    public partial class GuestDisplayName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreationTime",
                table: "Guest",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "DisplayName",
                table: "Guest",
                maxLength: 20,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreationTime",
                table: "Guest");

            migrationBuilder.DropColumn(
                name: "DisplayName",
                table: "Guest");
        }
    }
}
