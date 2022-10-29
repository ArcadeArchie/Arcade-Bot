using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ArcadeBot.Migrations
{
    public partial class NewQTCol : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "QuarantineHandleBehavior",
                table: "Servers",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "QuarantineHandleBehavior",
                table: "Servers");
        }
    }
}
