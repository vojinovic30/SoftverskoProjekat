using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SOFT.Migrations
{
    /// <inheritdoc />
    public partial class V2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ocena",
                table: "komentari");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ocena",
                table: "komentari",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
