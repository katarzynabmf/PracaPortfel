using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Portfel.Data.Migrations
{
    public partial class dodanoCehceAktywna : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Aktywna",
                table: "Uzytkownik",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Aktywna",
                table: "Transakcja",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Aktywna",
                table: "SymbolGieldowy",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Aktywna",
                table: "RodzajTransakcji",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Aktywna",
                table: "RodzajOplaty",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Aktywna",
                table: "Konto",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Aktywna",
                table: "Uzytkownik");

            migrationBuilder.DropColumn(
                name: "Aktywna",
                table: "Transakcja");

            migrationBuilder.DropColumn(
                name: "Aktywna",
                table: "SymbolGieldowy");

            migrationBuilder.DropColumn(
                name: "Aktywna",
                table: "RodzajTransakcji");

            migrationBuilder.DropColumn(
                name: "Aktywna",
                table: "RodzajOplaty");

            migrationBuilder.DropColumn(
                name: "Aktywna",
                table: "Konto");
        }
    }
}
