using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DevTalk.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addpreferncetable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Preferences",
                columns: table => new
                {
                    PreferenceId = table.Column<string>(type: "nvarchar(450)", nullable: false, defaultValueSql: "newid()"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Preferences", x => x.PreferenceId);
                    table.ForeignKey(
                        name: "FK_Preferences_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PreferenceCategory",
                columns: table => new
                {
                    CategoryId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PreferenceId = table.Column<string>(type: "nvarchar(450)", nullable: false)
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
                name: "IX_PreferenceCategory_CategoryId",
                table: "PreferenceCategory",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Preferences_UserId",
                table: "Preferences",
                column: "UserId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PreferenceCategory");

            migrationBuilder.DropTable(
                name: "Preferences");
        }
    }
}
