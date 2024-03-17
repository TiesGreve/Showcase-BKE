using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApi.Migrations
{
    /// <inheritdoc />
    public partial class GameFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Games_AspNetUsers_UserId",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "FirstMove",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "User1",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "User2",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "Winner",
                table: "Games");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Games",
                newName: "WinnerId");

            migrationBuilder.RenameIndex(
                name: "IX_Games_UserId",
                table: "Games",
                newName: "IX_Games_WinnerId");

            migrationBuilder.AddColumn<string>(
                name: "FirstMoveId",
                table: "Games",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "User1Id",
                table: "Games",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "User2Id",
                table: "Games",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Games_FirstMoveId",
                table: "Games",
                column: "FirstMoveId");

            migrationBuilder.CreateIndex(
                name: "IX_Games_User1Id",
                table: "Games",
                column: "User1Id");

            migrationBuilder.CreateIndex(
                name: "IX_Games_User2Id",
                table: "Games",
                column: "User2Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Games_AspNetUsers_FirstMoveId",
                table: "Games",
                column: "FirstMoveId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Games_AspNetUsers_User1Id",
                table: "Games",
                column: "User1Id",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Games_AspNetUsers_User2Id",
                table: "Games",
                column: "User2Id",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Games_AspNetUsers_WinnerId",
                table: "Games",
                column: "WinnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Games_AspNetUsers_FirstMoveId",
                table: "Games");

            migrationBuilder.DropForeignKey(
                name: "FK_Games_AspNetUsers_User1Id",
                table: "Games");

            migrationBuilder.DropForeignKey(
                name: "FK_Games_AspNetUsers_User2Id",
                table: "Games");

            migrationBuilder.DropForeignKey(
                name: "FK_Games_AspNetUsers_WinnerId",
                table: "Games");

            migrationBuilder.DropIndex(
                name: "IX_Games_FirstMoveId",
                table: "Games");

            migrationBuilder.DropIndex(
                name: "IX_Games_User1Id",
                table: "Games");

            migrationBuilder.DropIndex(
                name: "IX_Games_User2Id",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "FirstMoveId",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "User1Id",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "User2Id",
                table: "Games");

            migrationBuilder.RenameColumn(
                name: "WinnerId",
                table: "Games",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Games_WinnerId",
                table: "Games",
                newName: "IX_Games_UserId");

            migrationBuilder.AddColumn<string>(
                name: "FirstMove",
                table: "Games",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "User1",
                table: "Games",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "User2",
                table: "Games",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Winner",
                table: "Games",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Games_AspNetUsers_UserId",
                table: "Games",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
