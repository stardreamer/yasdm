using Microsoft.EntityFrameworkCore.Migrations;

namespace YASDM.Api.Migrations
{
    public partial class RoomstateWasAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "State",
                table: "Rooms",
                nullable: false,
                defaultValue: "Open");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "State",
                table: "Rooms");
        }
    }
}
