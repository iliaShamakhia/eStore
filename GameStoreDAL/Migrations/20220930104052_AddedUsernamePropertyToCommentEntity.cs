using Microsoft.EntityFrameworkCore.Migrations;

namespace GameStoreDAL.Migrations
{
    public partial class AddedUsernamePropertyToCommentEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Username",
                table: "Comments",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Username",
                table: "Comments");
        }
    }
}
