using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Portfel.Data.Migrations
{
    public partial class NowaWersjaPortfela : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                name: "KontoGotowkowe",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StanKonta = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Aktywna = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KontoGotowkowe", x => x.Id);
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
                        name: "FK_OperacjaGotowkowa_KontoGotowkowe_KontoGotowkoweId",
                        column: x => x.KontoGotowkoweId,
                        principalTable: "KontoGotowkowe",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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
                    KontoGotowkoweId = table.Column<int>(type: "int", nullable: false),
                    Aktywna = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Portfele", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Portfele_KontoGotowkowe_KontoGotowkoweId",
                        column: x => x.KontoGotowkoweId,
                        principalTable: "KontoGotowkowe",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Portfele_Uzytkownik_UzytkownikId",
                        column: x => x.UzytkownikId,
                        principalTable: "Uzytkownik",
                        principalColumn: "Id");
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
                    PortfelId = table.Column<int>(type: "int", nullable: false),
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
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OperacjaGotowkowa_KontoGotowkoweId",
                table: "OperacjaGotowkowa",
                column: "KontoGotowkoweId");

            migrationBuilder.CreateIndex(
                name: "IX_Portfele_KontoGotowkoweId",
                table: "Portfele",
                column: "KontoGotowkoweId",
                unique: true);

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
                name: "OperacjaGotowkowa");

            migrationBuilder.DropTable(
                name: "Pozycje");

            migrationBuilder.DropTable(
                name: "TransakcjeNew");

            migrationBuilder.DropTable(
                name: "Aktywa");

            migrationBuilder.DropTable(
                name: "Portfele");

            migrationBuilder.DropTable(
                name: "KontoGotowkowe");
        }
    }
}
