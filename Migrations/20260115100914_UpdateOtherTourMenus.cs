using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Umre.Web.Migrations
{
    /// <inheritdoc />
    public partial class UpdateOtherTourMenus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("UPDATE Menus SET Url = '/ozbekistan-turlari' WHERE Title = 'Özbekistan Turları'");
            migrationBuilder.Sql("UPDATE Menus SET Url = '/balkan-turlari' WHERE Title = 'Balkan Turları'");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
