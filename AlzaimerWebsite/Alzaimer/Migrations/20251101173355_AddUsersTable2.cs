using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AlzaimerApp.Migrations
{
    /// <inheritdoc />
    public partial class AddUsersTable2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Predictions");

            migrationBuilder.AlterColumn<float>(
                name: "Confidence",
                table: "Predictions",
                type: "real",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AddColumn<byte[]>(
                name: "ImageData",
                table: "Predictions",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0]);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageData",
                table: "Predictions");

            migrationBuilder.AlterColumn<double>(
                name: "Confidence",
                table: "Predictions",
                type: "float",
                nullable: false,
                oldClrType: typeof(float),
                oldType: "real");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Predictions",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
