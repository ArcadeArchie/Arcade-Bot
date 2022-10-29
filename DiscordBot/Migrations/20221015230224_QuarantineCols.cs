using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ArcadeBot.Migrations
{
    public partial class QuarantineCols : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "QuarantineEnabled",
                table: "Servers",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "QuarantineEnabled",
                table: "Servers");
        }
    }
}
