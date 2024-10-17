using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SOFT.Migrations
{
    /// <inheritdoc />
    public partial class V1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "mobilni",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    brand = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    model = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    tip = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mobilni", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    surname = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    username = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    password = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    admin = table.Column<bool>(type: "bit", nullable: false),
                    approved = table.Column<bool>(type: "bit", nullable: false),
                    picture = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    sigurnosnoPitanje = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    odgovorNaPitanje = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    brojPoena = table.Column<int>(type: "int", nullable: false),
                    brojTelefona = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "oglasi",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    mojMobilniid = table.Column<int>(type: "int", nullable: true),
                    zeljeniMobilniid = table.Column<int>(type: "int", nullable: true),
                    prihvacen = table.Column<bool>(type: "bit", nullable: false),
                    year = table.Column<int>(type: "int", nullable: true),
                    picture1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    picture2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    picture3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    price = table.Column<int>(type: "int", nullable: false),
                    color = table.Column<int>(type: "int", nullable: false),
                    memory = table.Column<int>(type: "int", nullable: false),
                    batteryHealth = table.Column<int>(type: "int", nullable: false),
                    userid = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_oglasi", x => x.id);
                    table.ForeignKey(
                        name: "FK_oglasi_mobilni_mojMobilniid",
                        column: x => x.mojMobilniid,
                        principalTable: "mobilni",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_oglasi_mobilni_zeljeniMobilniid",
                        column: x => x.zeljeniMobilniid,
                        principalTable: "mobilni",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_oglasi_users_userid",
                        column: x => x.userid,
                        principalTable: "users",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "favorites",
                columns: table => new
                {
                    FavoriteMobilniID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    userID = table.Column<int>(type: "int", nullable: false),
                    oglasID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_favorites", x => x.FavoriteMobilniID);
                    table.ForeignKey(
                        name: "FK_favorites_oglasi_oglasID",
                        column: x => x.oglasID,
                        principalTable: "oglasi",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_favorites_users_userID",
                        column: x => x.userID,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "komentari",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Datum = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Komentatorid = table.Column<int>(type: "int", nullable: true),
                    tekst = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    KomentarisanOglasid = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_komentari", x => x.Id);
                    table.ForeignKey(
                        name: "FK_komentari_oglasi_KomentarisanOglasid",
                        column: x => x.KomentarisanOglasid,
                        principalTable: "oglasi",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_komentari_users_Komentatorid",
                        column: x => x.Komentatorid,
                        principalTable: "users",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "requests",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    time = table.Column<DateTime>(type: "datetime2", nullable: false),
                    date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    senderid = table.Column<int>(type: "int", nullable: true),
                    recieverid = table.Column<int>(type: "int", nullable: true),
                    oglasid = table.Column<int>(type: "int", nullable: true),
                    odgovoren = table.Column<bool>(type: "bit", nullable: false),
                    prihvacen = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_requests", x => x.id);
                    table.ForeignKey(
                        name: "FK_requests_oglasi_oglasid",
                        column: x => x.oglasid,
                        principalTable: "oglasi",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_requests_users_recieverid",
                        column: x => x.recieverid,
                        principalTable: "users",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_requests_users_senderid",
                        column: x => x.senderid,
                        principalTable: "users",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "responses",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    requestid = table.Column<int>(type: "int", nullable: true),
                    accept = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_responses", x => x.id);
                    table.ForeignKey(
                        name: "FK_responses_requests_requestid",
                        column: x => x.requestid,
                        principalTable: "requests",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_favorites_oglasID",
                table: "favorites",
                column: "oglasID");

            migrationBuilder.CreateIndex(
                name: "IX_favorites_userID",
                table: "favorites",
                column: "userID");

            migrationBuilder.CreateIndex(
                name: "IX_komentari_KomentarisanOglasid",
                table: "komentari",
                column: "KomentarisanOglasid");

            migrationBuilder.CreateIndex(
                name: "IX_komentari_Komentatorid",
                table: "komentari",
                column: "Komentatorid");

            migrationBuilder.CreateIndex(
                name: "IX_oglasi_mojMobilniid",
                table: "oglasi",
                column: "mojMobilniid");

            migrationBuilder.CreateIndex(
                name: "IX_oglasi_userid",
                table: "oglasi",
                column: "userid");

            migrationBuilder.CreateIndex(
                name: "IX_oglasi_zeljeniMobilniid",
                table: "oglasi",
                column: "zeljeniMobilniid");

            migrationBuilder.CreateIndex(
                name: "IX_requests_oglasid",
                table: "requests",
                column: "oglasid");

            migrationBuilder.CreateIndex(
                name: "IX_requests_recieverid",
                table: "requests",
                column: "recieverid");

            migrationBuilder.CreateIndex(
                name: "IX_requests_senderid",
                table: "requests",
                column: "senderid");

            migrationBuilder.CreateIndex(
                name: "IX_responses_requestid",
                table: "responses",
                column: "requestid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "favorites");

            migrationBuilder.DropTable(
                name: "komentari");

            migrationBuilder.DropTable(
                name: "responses");

            migrationBuilder.DropTable(
                name: "requests");

            migrationBuilder.DropTable(
                name: "oglasi");

            migrationBuilder.DropTable(
                name: "mobilni");

            migrationBuilder.DropTable(
                name: "users");
        }
    }
}
