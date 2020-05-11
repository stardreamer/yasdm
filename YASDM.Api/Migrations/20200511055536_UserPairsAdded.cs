using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace YASDM.Api.Migrations
{
    public partial class UserPairsAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserPairs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RoomId = table.Column<int>(nullable: false),
                    User1Id = table.Column<int>(nullable: false),
                    User2Id = table.Column<int>(nullable: false),
                    User1Alias = table.Column<string>(nullable: true),
                    User2Alias = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserPairs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserPairs_Rooms_RoomId",
                        column: x => x.RoomId,
                        principalTable: "Rooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserPairs_RoomId_User1Id",
                table: "UserPairs",
                columns: new[] { "RoomId", "User1Id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserPairs_RoomId_User2Id",
                table: "UserPairs",
                columns: new[] { "RoomId", "User2Id" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserPairs");
        }
    }
}
