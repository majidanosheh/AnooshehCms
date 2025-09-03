using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication16.Migrations
{
    /// <inheritdoc />
    public partial class AllowedFileTypeTODataBase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "allowedFileTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Extension = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Signatures = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "امضاهای مختلف را با کاما جدا کنید. مثال: FFD8FFE0,FFD8FFE1"),
                    MimeType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MaxSizeInBytes = table.Column<long>(type: "bigint", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_allowedFileTypes", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "allowedFileTypes");
        }
    }
}
