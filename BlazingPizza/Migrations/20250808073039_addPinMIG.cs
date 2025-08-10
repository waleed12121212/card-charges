using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlazingPizza.Migrations
{
    /// <inheritdoc />
    public partial class addPinMIG : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PIN",
                table: "Users",
                newName: "PinHash");

            migrationBuilder.AddColumn<bool>(
                name: "IsPinRequired",
                table: "Users",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "PinAttempts",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "PinLastUsed",
                table: "Users",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PinLockedUntil",
                table: "Users",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPinRequired",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "PinAttempts",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "PinLastUsed",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "PinLockedUntil",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "PinHash",
                table: "Users",
                newName: "PIN");
        }
    }
}
