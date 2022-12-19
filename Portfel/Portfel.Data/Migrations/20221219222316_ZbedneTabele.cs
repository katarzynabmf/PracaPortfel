using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Portfel.Data.Migrations
{
    public partial class ZbedneTabele : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Transakcja");

            migrationBuilder.DropTable(
                name: "Konto");

            migrationBuilder.DropTable(
                name: "RodzajOplaty");

            migrationBuilder.DropTable(
                name: "RodzajTransakcji");

            migrationBuilder.DropTable(
                name: "SymbolGieldowy");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Konto",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UzytkownikId = table.Column<int>(type: "int", nullable: true),
                    Aktywna = table.Column<bool>(type: "bit", nullable: false),
                    Gotowka = table.Column<double>(type: "float", nullable: false),
                    Nazwa = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Waluta = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Konto", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Konto_Uzytkownik_UzytkownikId",
                        column: x => x.UzytkownikId,
                        principalTable: "Uzytkownik",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "RodzajOplaty",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Aktywna = table.Column<bool>(type: "bit", nullable: false),
                    Nazwa = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Promowany = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RodzajOplaty", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RodzajTransakcji",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Aktywna = table.Column<bool>(type: "bit", nullable: false),
                    Nazwa = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RodzajTransakcji", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SymbolGieldowy",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Aktywna = table.Column<bool>(type: "bit", nullable: false),
                    Nazwa = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Opis = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SymbolGieldowy", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Transakcja",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KontoId = table.Column<int>(type: "int", nullable: false),
                    RodzajOplatyId = table.Column<int>(type: "int", nullable: true),
                    RodzajTransakcjiId = table.Column<int>(type: "int", nullable: false),
                    SymbolGieldowyId = table.Column<int>(type: "int", nullable: true),
                    Aktywna = table.Column<bool>(type: "bit", nullable: false),
                    DataUtworzenia = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Ilosc = table.Column<int>(type: "int", nullable: false),
                    IloscRodzajuOplaty = table.Column<int>(type: "int", nullable: false),
                    Komentarz = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Kwota = table.Column<double>(type: "float", nullable: false),
                    Waluta = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transakcja", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Transakcja_Konto_KontoId",
                        column: x => x.KontoId,
                        principalTable: "Konto",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Transakcja_RodzajOplaty_RodzajOplatyId",
                        column: x => x.RodzajOplatyId,
                        principalTable: "RodzajOplaty",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Transakcja_RodzajTransakcji_RodzajTransakcjiId",
                        column: x => x.RodzajTransakcjiId,
                        principalTable: "RodzajTransakcji",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Transakcja_SymbolGieldowy_SymbolGieldowyId",
                        column: x => x.SymbolGieldowyId,
                        principalTable: "SymbolGieldowy",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Konto_UzytkownikId",
                table: "Konto",
                column: "UzytkownikId");

            migrationBuilder.CreateIndex(
                name: "IX_Transakcja_KontoId",
                table: "Transakcja",
                column: "KontoId");

            migrationBuilder.CreateIndex(
                name: "IX_Transakcja_RodzajOplatyId",
                table: "Transakcja",
                column: "RodzajOplatyId");

            migrationBuilder.CreateIndex(
                name: "IX_Transakcja_RodzajTransakcjiId",
                table: "Transakcja",
                column: "RodzajTransakcjiId");

            migrationBuilder.CreateIndex(
                name: "IX_Transakcja_SymbolGieldowyId",
                table: "Transakcja",
                column: "SymbolGieldowyId");
        }
    }
}
