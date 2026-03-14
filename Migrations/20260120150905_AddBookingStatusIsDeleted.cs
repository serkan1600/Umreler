using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Umre.Web.Migrations
{
    /// <inheritdoc />
    public partial class AddBookingStatusIsDeleted : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "UmreRezervasyonlari",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "UmreRezervasyonlari",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Rezervasyonlar",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Rezervasyonlar",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "AramaTalepleri",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "AramaTalepleri",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "UmreRezervasyonlari");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "UmreRezervasyonlari");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Rezervasyonlar");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Rezervasyonlar");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "AramaTalepleri");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "AramaTalepleri");
        }
    }
}
