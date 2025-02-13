using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DevTalk.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class applyindexonthreevaluesonposts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Posts_PopularityScore",
                table: "Posts");

            migrationBuilder.CreateIndex(
                name: "IX_Posts_PopularityScore_PostedAt_PostId",
                table: "Posts",
                columns: new[] { "PopularityScore", "PostedAt", "PostId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Posts_PopularityScore_PostedAt_PostId",
                table: "Posts");

            migrationBuilder.CreateIndex(
                name: "IX_Posts_PopularityScore",
                table: "Posts",
                column: "PopularityScore");
        }
    }
}
