using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YourMoviesForum.Data.Migrations
{
    public partial class AddedAuthorToPosts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Replies_Posts_PostId1",
                table: "Replies");

            migrationBuilder.DropIndex(
                name: "IX_Replies_PostId1",
                table: "Replies");

            migrationBuilder.DropColumn(
                name: "PostId1",
                table: "Replies");

            migrationBuilder.AlterColumn<int>(
                name: "PostId",
                table: "Replies",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Replies_PostId",
                table: "Replies",
                column: "PostId");

            migrationBuilder.AddForeignKey(
                name: "FK_Replies_Posts_PostId",
                table: "Replies",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Replies_Posts_PostId",
                table: "Replies");

            migrationBuilder.DropIndex(
                name: "IX_Replies_PostId",
                table: "Replies");

            migrationBuilder.AlterColumn<string>(
                name: "PostId",
                table: "Replies",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "PostId1",
                table: "Replies",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Replies_PostId1",
                table: "Replies",
                column: "PostId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Replies_Posts_PostId1",
                table: "Replies",
                column: "PostId1",
                principalTable: "Posts",
                principalColumn: "Id");
        }
    }
}
