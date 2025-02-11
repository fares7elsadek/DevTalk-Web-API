using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DevTalk.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updatepreferencestalbe : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PreferenceCategory");

            migrationBuilder.DropIndex(
                name: "IX_Preferences_UserId",
                table: "Preferences");

            migrationBuilder.AddColumn<string>(
                name: "CategoryId",
                table: "Preferences",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Preferences_CategoryId",
                table: "Preferences",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Preferences_UserId",
                table: "Preferences",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Preferences_Category_CategoryId",
                table: "Preferences",
                column: "CategoryId",
                principalTable: "Category",
                principalColumn: "CategoryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Preferences_Category_CategoryId",
                table: "Preferences");

            migrationBuilder.DropIndex(
                name: "IX_Preferences_CategoryId",
                table: "Preferences");

            migrationBuilder.DropIndex(
                name: "IX_Preferences_UserId",
                table: "Preferences");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "Preferences");

            migrationBuilder.CreateTable(
                name: "PreferenceCategory",
                columns: table => new
                {
                    PreferenceId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CategoryId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PreferenceCategory", x => new { x.PreferenceId, x.CategoryId });
                    table.ForeignKey(
                        name: "FK_PreferenceCategory_Category_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Category",
                        principalColumn: "CategoryId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PreferenceCategory_Preferences_PreferenceId",
                        column: x => x.PreferenceId,
                        principalTable: "Preferences",
                        principalColumn: "PreferenceId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Preferences_UserId",
                table: "Preferences",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PreferenceCategory_CategoryId",
                table: "PreferenceCategory",
                column: "CategoryId");
        }
    }
}
