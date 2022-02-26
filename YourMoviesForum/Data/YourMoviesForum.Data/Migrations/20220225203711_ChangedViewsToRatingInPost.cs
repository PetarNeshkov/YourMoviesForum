using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YourMoviesForum.Data.Migrations
{
    public partial class ChangedViewsToRatingInPost : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Views",
                table: "Posts",
                newName: "Rating");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Rating",
                table: "Posts",
                newName: "Views");
        }
    }
}
