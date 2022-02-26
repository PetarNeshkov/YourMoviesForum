using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YourMoviesForum.Data.Migrations
{
    public partial class AddedRatingToUsers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Rating",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Rating",
                table: "AspNetUsers");
        }
    }
}
