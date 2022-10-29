using Microsoft.EntityFrameworkCore.Migrations;

namespace ArcadeBot.Migrations
{
    public partial class ConfigOverridesMig : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<ulong>(
                name: "TwitchNotificationChannel",
                table: "Servers",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0ul);

            migrationBuilder.CreateTable(
                name: "ServerConfigOverride",
                columns: table => new
                {
                    Key = table.Column<string>(type: "TEXT", nullable: false),
                    ServerId = table.Column<ulong>(type: "INTEGER", nullable: true),
                    ConfigKey = table.Column<string>(type: "TEXT", nullable: true),
                    OverrideValue = table.Column<string>(type: "TEXT", nullable: true),
                    Active = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServerConfigOverride", x => x.Key);
                    table.ForeignKey(
                        name: "FK_ServerConfigOverride_Servers_ServerId",
                        column: x => x.ServerId,
                        principalTable: "Servers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ServerConfigOverride_ServerId",
                table: "ServerConfigOverride",
                column: "ServerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ServerConfigOverride");

            migrationBuilder.DropColumn(
                name: "TwitchNotificationChannel",
                table: "Servers");
        }
    }
}
