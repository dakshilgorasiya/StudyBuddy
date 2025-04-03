using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudyBuddy.Migrations
{
    /// <inheritdoc />
    public partial class RemovePictureFromCommentModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Pictures",
                table: "Comments");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Pictures",
                table: "Comments",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
