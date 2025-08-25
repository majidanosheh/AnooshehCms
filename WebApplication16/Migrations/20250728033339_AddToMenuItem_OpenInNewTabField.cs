using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication16.Migrations
{
    /// <inheritdoc />
    public partial class AddToMenuItem_OpenInNewTabField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "OpenInNewTab",
                table: "MenuItems",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OpenInNewTab",
                table: "MenuItems");
        }
    }
}
