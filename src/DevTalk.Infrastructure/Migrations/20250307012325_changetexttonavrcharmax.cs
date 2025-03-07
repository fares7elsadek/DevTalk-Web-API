using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DevTalk.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class changetexttonavrcharmax : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MediaFileName",
                table: "PostMedia");

            migrationBuilder.RenameColumn(
                name: "MediaUrl",
                table: "PostMedia",
                newName: "MediaPath");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "MediaPath",
                table: "PostMedia",
                newName: "MediaUrl");

            migrationBuilder.AddColumn<string>(
                name: "MediaFileName",
                table: "PostMedia",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
