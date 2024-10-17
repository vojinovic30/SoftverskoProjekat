using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SOFT.Migrations
{
    /// <inheritdoc />
    public partial class V3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ocene",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ocenioid = table.Column<int>(type: "int", nullable: true),
                    ocena = table.Column<int>(type: "int", nullable: false),
                    ocenjenid = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ocene", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ocene_users_ocenioid",
                        column: x => x.ocenioid,
                        principalTable: "users",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_ocene_users_ocenjenid",
                        column: x => x.ocenjenid,
                        principalTable: "users",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ocene_ocenioid",
                table: "ocene",
                column: "ocenioid");

            migrationBuilder.CreateIndex(
                name: "IX_ocene_ocenjenid",
                table: "ocene",
                column: "ocenjenid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ocene");
        }
    }
}
