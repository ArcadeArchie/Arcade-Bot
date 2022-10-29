using Microsoft.EntityFrameworkCore.Migrations;

namespace ArcadeBot.Migrations
{
    public partial class DefaultRoleCol : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<ulong>(
                name: "DefaultRole",
                table: "Servers",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0ul);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DefaultRole",
                table: "Servers");
        }
    }
}
