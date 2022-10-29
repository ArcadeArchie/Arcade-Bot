using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ArcadeBot.Migrations
{
    public partial class NewServerCols16102022 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Quotes");

            migrationBuilder.RenameColumn(
                name: "TwitchNotificationChannel",
                table: "Servers",
                newName: "QuarantineRole");

            migrationBuilder.AddColumn<ulong>(
                name: "QuarantineChannel",
                table: "Servers",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0ul);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "QuarantineChannel",
                table: "Servers");

            migrationBuilder.RenameColumn(
                name: "QuarantineRole",
                table: "Servers",
                newName: "TwitchNotificationChannel");

            migrationBuilder.CreateTable(
                name: "Quotes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AuthorId = table.Column<string>(type: "TEXT", nullable: true),
                    AuthorName = table.Column<string>(type: "TEXT", nullable: true),
                    Content = table.Column<string>(type: "TEXT", nullable: true),
                    Posted_on = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                    ServerId = table.Column<string>(type: "TEXT", nullable: true),
                    ServerName = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Quotes", x => x.Id);
                });
        }
    }
}
