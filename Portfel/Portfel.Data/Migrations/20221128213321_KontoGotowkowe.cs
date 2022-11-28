using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Portfel.Data.Migrations
{
    public partial class KontoGotowkowe : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Portfele_KontoGotowkowe_KontoGotowkoweId",
                table: "Portfele");

            migrationBuilder.DropForeignKey(
                name: "FK_TransakcjeNew_Portfele_PortfelId",
                table: "TransakcjeNew");

            migrationBuilder.DropIndex(
                name: "IX_Portfele_KontoGotowkoweId",
                table: "Portfele");

            migrationBuilder.AlterColumn<int>(
                name: "PortfelId",
                table: "TransakcjeNew",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "KontoGotowkoweId",
                table: "Portfele",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_Portfele_KontoGotowkoweId",
                table: "Portfele",
                column: "KontoGotowkoweId",
                unique: true,
                filter: "[KontoGotowkoweId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Portfele_KontoGotowkowe_KontoGotowkoweId",
                table: "Portfele",
                column: "KontoGotowkoweId",
                principalTable: "KontoGotowkowe",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TransakcjeNew_Portfele_PortfelId",
                table: "TransakcjeNew",
                column: "PortfelId",
                principalTable: "Portfele",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Portfele_KontoGotowkowe_KontoGotowkoweId",
                table: "Portfele");

            migrationBuilder.DropForeignKey(
                name: "FK_TransakcjeNew_Portfele_PortfelId",
                table: "TransakcjeNew");

            migrationBuilder.DropIndex(
                name: "IX_Portfele_KontoGotowkoweId",
                table: "Portfele");

            migrationBuilder.AlterColumn<int>(
                name: "PortfelId",
                table: "TransakcjeNew",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "KontoGotowkoweId",
                table: "Portfele",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Portfele_KontoGotowkoweId",
                table: "Portfele",
                column: "KontoGotowkoweId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Portfele_KontoGotowkowe_KontoGotowkoweId",
                table: "Portfele",
                column: "KontoGotowkoweId",
                principalTable: "KontoGotowkowe",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TransakcjeNew_Portfele_PortfelId",
                table: "TransakcjeNew",
                column: "PortfelId",
                principalTable: "Portfele",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
