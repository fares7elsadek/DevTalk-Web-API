using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DevTalk.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddDeletePostProcedure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                CREATE PROCEDURE DeletePostWithRelations
                        @PostId nvarchar(450),@Status INT OUTPUT
                    AS
                    BEGIN
                        SET NOCOUNT ON;
                        BEGIN TRY
                            BEGIN TRANSACTION;

                            -- Delete related data first
                            DELETE FROM PostMedia WHERE PostId = @PostId;
                            DELETE FROM Comments WHERE PostId = @PostId;
                            DELETE FROM PostVotes WHERE PostId = @PostId;
                            DELETE FROM Bookmarks WHERE PostId = @PostId;

                            -- Delete the post itself
                            DELETE FROM Posts WHERE PostId = @PostId;

                            COMMIT TRANSACTION;
		                    SET @Status = 1; -- Success
                        END TRY
                        BEGIN CATCH
                            -- Check if a transaction is open before rolling back
                            IF @@TRANCOUNT > 0
                                ROLLBACK TRANSACTION;

		                    SET @Status = -1; -- Failure
                            THROW;
                        END CATCH
                    END;
                ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS DeletePostWithRelations;");
        }
    }
}
