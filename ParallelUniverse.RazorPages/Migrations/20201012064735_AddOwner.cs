using Microsoft.EntityFrameworkCore.Migrations;

namespace ParallelUniverse.RazorPages.Migrations
{
    public partial class AddOwner : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GuestId",
                table: "FileResource",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Guest_Name",
                table: "Guest",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FileResource_GuestId",
                table: "FileResource",
                column: "GuestId");

            migrationBuilder.CreateIndex(
                name: "IX_FileResource_Name",
                table: "FileResource",
                column: "Name");

            migrationBuilder.AddForeignKey(
                name: "FK_FileResource_Guest_GuestId",
                table: "FileResource",
                column: "GuestId",
                principalTable: "Guest",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FileResource_Guest_GuestId",
                table: "FileResource");

            migrationBuilder.DropIndex(
                name: "IX_Guest_Name",
                table: "Guest");

            migrationBuilder.DropIndex(
                name: "IX_FileResource_GuestId",
                table: "FileResource");

            migrationBuilder.DropIndex(
                name: "IX_FileResource_Name",
                table: "FileResource");

            migrationBuilder.DropColumn(
                name: "GuestId",
                table: "FileResource");
        }
    }
}
