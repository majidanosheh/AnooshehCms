using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication16.Migrations
{
    /// <inheritdoc />
    public partial class AddMenuManagementEntitie_Edit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Menus",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Menus",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedAt",
                table: "Menus",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModifiedBy",
                table: "Menus",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Menus");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Menus");

            migrationBuilder.DropColumn(
                name: "ModifiedAt",
                table: "Menus");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "Menus");
        }
    }
}
