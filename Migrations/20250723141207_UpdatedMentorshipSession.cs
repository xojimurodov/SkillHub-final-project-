using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SkillHub.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedMentorshipSession : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Tags",
                table: "MentorshipSessions");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Tags",
                table: "MentorshipSessions",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }
    }
}
