using Microsoft.EntityFrameworkCore.Migrations;

namespace ArcadeBot.Migrations
{
    public partial class UpdatedServerUserER : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Servers_Users_ServerUserId",
                table: "Servers");

            migrationBuilder.DropIndex(
                name: "IX_Servers_ServerUserId",
                table: "Servers");

            migrationBuilder.DropColumn(
                name: "ServerUserId",
                table: "Servers");

            migrationBuilder.CreateTable(
                name: "ServerServerUser",
                columns: table => new
                {
                    GuildsId = table.Column<ulong>(type: "INTEGER", nullable: false),
                    UsersId = table.Column<ulong>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServerServerUser", x => new { x.GuildsId, x.UsersId });
                    table.ForeignKey(
                        name: "FK_ServerServerUser_Servers_GuildsId",
                        column: x => x.GuildsId,
                        principalTable: "Servers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ServerServerUser_Users_UsersId",
                        column: x => x.UsersId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ServerServerUser_UsersId",
                table: "ServerServerUser",
                column: "UsersId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ServerServerUser");

            migrationBuilder.AddColumn<ulong>(
                name: "ServerUserId",
                table: "Servers",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Servers_ServerUserId",
                table: "Servers",
                column: "ServerUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Servers_Users_ServerUserId",
                table: "Servers",
                column: "ServerUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
