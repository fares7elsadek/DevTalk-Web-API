using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DevTalk.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addavatartotheuser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Avatar",
                table: "AspNetUsers",
                newName: "AvatarUrl");

            migrationBuilder.AddColumn<string>(
                name: "AvatarFileName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AvatarFileName",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "AvatarUrl",
                table: "AspNetUsers",
                newName: "Avatar");
        }
    }
}
