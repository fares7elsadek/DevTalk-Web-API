using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DevTalk.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class applyindexonpopularityscore : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Posts_PopularityScore",
                table: "Posts",
                column: "PopularityScore");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Posts_PopularityScore",
                table: "Posts");
        }
    }
}
