using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Portfel.Intranet.Migrations
{
    public partial class DodanoKolejneTabele : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Kontos_Uzytkownik_UzytkownikId",
                table: "Kontos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Kontos",
                table: "Kontos");

            migrationBuilder.RenameTable(
                name: "Kontos",
                newName: "Konto");

            migrationBuilder.RenameIndex(
                name: "IX_Kontos_UzytkownikId",
                table: "Konto",
                newName: "IX_Konto_UzytkownikId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Konto",
                table: "Konto",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "RodzajOplaty",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
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
                    Nazwa = table.Column<string>(type: "nvarchar(max)", nullable: false)
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
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RodzajTransakcjiId = table.Column<int>(type: "int", nullable: false),
                    Waluta = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SymbolGieldowyId = table.Column<int>(type: "int", nullable: true),
                    Kwota = table.Column<double>(type: "float", nullable: false),
                    Ilosc = table.Column<int>(type: "int", nullable: false),
                    RodzajOplatyId = table.Column<int>(type: "int", nullable: true),
                    Komentarz = table.Column<string>(type: "nvarchar(max)", nullable: false)
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

            migrationBuilder.AddForeignKey(
                name: "FK_Konto_Uzytkownik_UzytkownikId",
                table: "Konto",
                column: "UzytkownikId",
                principalTable: "Uzytkownik",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Konto_Uzytkownik_UzytkownikId",
                table: "Konto");

            migrationBuilder.DropTable(
                name: "Transakcja");

            migrationBuilder.DropTable(
                name: "RodzajOplaty");

            migrationBuilder.DropTable(
                name: "RodzajTransakcji");

            migrationBuilder.DropTable(
                name: "SymbolGieldowy");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Konto",
                table: "Konto");

            migrationBuilder.RenameTable(
                name: "Konto",
                newName: "Kontos");

            migrationBuilder.RenameIndex(
                name: "IX_Konto_UzytkownikId",
                table: "Kontos",
                newName: "IX_Kontos_UzytkownikId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Kontos",
                table: "Kontos",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Kontos_Uzytkownik_UzytkownikId",
                table: "Kontos",
                column: "UzytkownikId",
                principalTable: "Uzytkownik",
                principalColumn: "Id");
        }
    }
}
