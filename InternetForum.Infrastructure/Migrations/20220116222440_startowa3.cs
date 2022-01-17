using Microsoft.EntityFrameworkCore.Migrations;

namespace InternetForum.Infrastructure.Migrations
{
    public partial class startowa3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reply_Post_PostId",
                table: "Reply");

            migrationBuilder.AddForeignKey(
                name: "FK_Reply_Post_PostId",
                table: "Reply",
                column: "PostId",
                principalTable: "Post",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reply_Post_PostId",
                table: "Reply");

            migrationBuilder.AddForeignKey(
                name: "FK_Reply_Post_PostId",
                table: "Reply",
                column: "PostId",
                principalTable: "Post",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
