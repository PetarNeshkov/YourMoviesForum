using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YourMoviesForum.Data.Migrations
{
    public partial class AddedParentIdToReply : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ParentId",
                table: "Replies",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Replies_ParentId",
                table: "Replies",
                column: "ParentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Replies_Replies_ParentId",
                table: "Replies",
                column: "ParentId",
                principalTable: "Replies",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Replies_Replies_ParentId",
                table: "Replies");

            migrationBuilder.DropIndex(
                name: "IX_Replies_ParentId",
                table: "Replies");

            migrationBuilder.DropColumn(
                name: "ParentId",
                table: "Replies");
        }
    }
}
