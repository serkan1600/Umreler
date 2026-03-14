using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Umre.Web.Migrations
{
    /// <inheritdoc />
    public partial class SyncPendingModelChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PackageId",
                table: "UmreRezervasyonlari",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UmreRezervasyonlari_PackageId",
                table: "UmreRezervasyonlari",
                column: "PackageId");

            migrationBuilder.AddForeignKey(
                name: "FK_UmreRezervasyonlari_Paketler_PackageId",
                table: "UmreRezervasyonlari",
                column: "PackageId",
                principalTable: "Paketler",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UmreRezervasyonlari_Paketler_PackageId",
                table: "UmreRezervasyonlari");

            migrationBuilder.DropIndex(
                name: "IX_UmreRezervasyonlari_PackageId",
                table: "UmreRezervasyonlari");

            migrationBuilder.DropColumn(
                name: "PackageId",
                table: "UmreRezervasyonlari");
        }
    }
}
