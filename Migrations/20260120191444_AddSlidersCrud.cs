using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Umre.Web.Migrations
{
    /// <inheritdoc />
    public partial class AddSlidersCrud : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LinkUrl",
                table: "Slaytlar");

            migrationBuilder.RenameColumn(
                name: "Subtitle",
                table: "Slaytlar",
                newName: "SubTitle");

            migrationBuilder.RenameColumn(
                name: "Order",
                table: "Slaytlar",
                newName: "SortOrder");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Slaytlar",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Slaytlar");

            migrationBuilder.RenameColumn(
                name: "SubTitle",
                table: "Slaytlar",
                newName: "Subtitle");

            migrationBuilder.RenameColumn(
                name: "SortOrder",
                table: "Slaytlar",
                newName: "Order");

            migrationBuilder.AddColumn<string>(
                name: "LinkUrl",
                table: "Slaytlar",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
