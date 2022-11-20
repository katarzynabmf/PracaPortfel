using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Portfel.Data.Migrations
{
    public partial class TransakcjUzytkownikZmiany : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DataUtworzenia",
                table: "Uzytkownik",
                type: "datetime2",
                nullable: false,
                defaultValue: DateTime.Now);

            migrationBuilder.AddColumn<DateTime>(
                name: "DataUtworzenia",
                table: "Transakcja",
                type: "datetime2",
                nullable: false,
                defaultValue: DateTime.Now);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DataUtworzenia",
                table: "Uzytkownik");

            migrationBuilder.DropColumn(
                name: "DataUtworzenia",
                table: "Transakcja");
        }
    }
}
