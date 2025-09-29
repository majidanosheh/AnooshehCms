using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication16.Migrations
{
    /// <inheritdoc />
    public partial class AddFormBuilderFeatures : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ValidationRulesJson",
                table: "FormFields",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ValidationRulesJson",
                table: "FormFields");
        }
    }
}
