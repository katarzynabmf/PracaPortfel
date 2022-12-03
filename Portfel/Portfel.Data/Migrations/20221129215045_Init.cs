using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Portfel.Data.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Aktualnosc",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Pozycja = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Tytul = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Tresc = table.Column<string>(type: "nvarchar(MAX)", nullable: false),
                    DataDodania = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FotoUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Priorytet = table.Column<long>(type: "bigint", nullable: false),
                    Aktywna = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Aktualnosc", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Aktywa",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Symbol = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Nazwa = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CenaAktualna = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Aktywna = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Aktywa", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RodzajOplaty",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nazwa = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Aktywna = table.Column<bool>(type: "bit", nullable: false),
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
                    Nazwa = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Aktywna = table.Column<bool>(type: "bit", nullable: false)
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
                    Nazwa = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Opis = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Aktywna = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SymbolGieldowy", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Uzytkownik",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Imie = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Haslo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Aktywna = table.Column<bool>(type: "bit", nullable: false),
                    DataUtworzenia = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Uzytkownik", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Konto",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nazwa = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Waluta = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Gotowka = table.Column<double>(type: "float", nullable: false),
                    UzytkownikId = table.Column<int>(type: "int", nullable: true),
                    Aktywna = table.Column<bool>(type: "bit", nullable: false)
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
                name: "Portfele",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nazwa = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Waluta = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UzytkownikId = table.Column<int>(type: "int", nullable: true),
                    Aktywna = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Portfele", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Portfele_Uzytkownik_UzytkownikId",
                        column: x => x.UzytkownikId,
                        principalTable: "Uzytkownik",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Transakcja",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KontoId = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RodzajTransakcjiId = table.Column<int>(type: "int", nullable: false),
                    Waluta = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SymbolGieldowyId = table.Column<int>(type: "int", nullable: true),
                    Kwota = table.Column<double>(type: "float", nullable: false),
                    Ilosc = table.Column<int>(type: "int", nullable: false),
                    RodzajOplatyId = table.Column<int>(type: "int", nullable: true),
                    IloscRodzajuOplaty = table.Column<int>(type: "int", nullable: false),
                    Komentarz = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Aktywna = table.Column<bool>(type: "bit", nullable: false),
                    DataUtworzenia = table.Column<DateTime>(type: "datetime2", nullable: false)
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

            migrationBuilder.CreateTable(
                name: "KontaGotowkowe",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PortfelId = table.Column<int>(type: "int", nullable: false),
                    StanKonta = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Aktywna = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KontaGotowkowe", x => x.Id);
                    table.ForeignKey(
                        name: "FK_KontaGotowkowe_Portfele_PortfelId",
                        column: x => x.PortfelId,
                        principalTable: "Portfele",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Pozycje",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AktywoId = table.Column<int>(type: "int", nullable: false),
                    Ilosc = table.Column<long>(type: "bigint", nullable: false),
                    SredniaCenaZakupu = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PortfelId = table.Column<int>(type: "int", nullable: true),
                    Aktywna = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pozycje", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Pozycje_Aktywa_AktywoId",
                        column: x => x.AktywoId,
                        principalTable: "Aktywa",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Pozycje_Portfele_PortfelId",
                        column: x => x.PortfelId,
                        principalTable: "Portfele",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TransakcjeNew",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Kierunek = table.Column<int>(type: "int", nullable: false),
                    AktywoId = table.Column<int>(type: "int", nullable: false),
                    PortfelId = table.Column<int>(type: "int", nullable: true),
                    Cena = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DataTransakcji = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Komentarz = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Aktywna = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransakcjeNew", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TransakcjeNew_Aktywa_AktywoId",
                        column: x => x.AktywoId,
                        principalTable: "Aktywa",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TransakcjeNew_Portfele_PortfelId",
                        column: x => x.PortfelId,
                        principalTable: "Portfele",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "OperacjaGotowkowa",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KontoGotowkoweId = table.Column<int>(type: "int", nullable: false),
                    DataOperacji = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Kwota = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TypOperacjiGotowkowej = table.Column<int>(type: "int", nullable: false),
                    Aktywna = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OperacjaGotowkowa", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OperacjaGotowkowa_KontaGotowkowe_KontoGotowkoweId",
                        column: x => x.KontoGotowkoweId,
                        principalTable: "KontaGotowkowe",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_KontaGotowkowe_PortfelId",
                table: "KontaGotowkowe",
                column: "PortfelId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Konto_UzytkownikId",
                table: "Konto",
                column: "UzytkownikId");

            migrationBuilder.CreateIndex(
                name: "IX_OperacjaGotowkowa_KontoGotowkoweId",
                table: "OperacjaGotowkowa",
                column: "KontoGotowkoweId");

            migrationBuilder.CreateIndex(
                name: "IX_Portfele_UzytkownikId",
                table: "Portfele",
                column: "UzytkownikId");

            migrationBuilder.CreateIndex(
                name: "IX_Pozycje_AktywoId",
                table: "Pozycje",
                column: "AktywoId");

            migrationBuilder.CreateIndex(
                name: "IX_Pozycje_PortfelId",
                table: "Pozycje",
                column: "PortfelId");

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

            migrationBuilder.CreateIndex(
                name: "IX_TransakcjeNew_AktywoId",
                table: "TransakcjeNew",
                column: "AktywoId");

            migrationBuilder.CreateIndex(
                name: "IX_TransakcjeNew_PortfelId",
                table: "TransakcjeNew",
                column: "PortfelId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Aktualnosc");

            migrationBuilder.DropTable(
                name: "OperacjaGotowkowa");

            migrationBuilder.DropTable(
                name: "Pozycje");

            migrationBuilder.DropTable(
                name: "Transakcja");

            migrationBuilder.DropTable(
                name: "TransakcjeNew");

            migrationBuilder.DropTable(
                name: "KontaGotowkowe");

            migrationBuilder.DropTable(
                name: "Konto");

            migrationBuilder.DropTable(
                name: "RodzajOplaty");

            migrationBuilder.DropTable(
                name: "RodzajTransakcji");

            migrationBuilder.DropTable(
                name: "SymbolGieldowy");

            migrationBuilder.DropTable(
                name: "Aktywa");

            migrationBuilder.DropTable(
                name: "Portfele");

            migrationBuilder.DropTable(
                name: "Uzytkownik");
        }
    }
}
