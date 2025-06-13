using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApi.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedAtCreatedAt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("e31f281a-a853-4c0b-a7e0-778a0b17b2f9"));

            migrationBuilder.DropColumn(
                name: "GameCreation",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "GameFinish",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "GameStart",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "GameUpdate",
                table: "Games");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Games",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "FinishedAt",
                table: "Games",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "StartedAt",
                table: "Games",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Games",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "AspNetUsers",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "AspNetUsers",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { new Guid("99949f2e-bcaf-4217-a217-80bd1bcbb92f"), "13/06/2025 11:23:40", "Admin", "ADMIN" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("99949f2e-bcaf-4217-a217-80bd1bcbb92f"));

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "FinishedAt",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "StartedAt",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<DateTime>(
                name: "GameCreation",
                table: "Games",
                type: "timestamp",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "GameFinish",
                table: "Games",
                type: "timestamp",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "GameStart",
                table: "Games",
                type: "timestamp",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "GameUpdate",
                table: "Games",
                type: "timestamp",
                nullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { new Guid("e31f281a-a853-4c0b-a7e0-778a0b17b2f9"), "15/05/2025 07:12:16", "Admin", "ADMIN" });
        }
    }
}
