using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Umre.Web.Migrations
{
    /// <inheritdoc />
    public partial class AddUmreKayitToHacMenu : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                INSERT INTO Menus (Title, Url, [Order], IsActive, IsAdmin, ParentId)
                SELECT 'Umre Kayıt', '/umre-kayit', 1, 1, 0, Id 
                FROM Menus 
                WHERE Title = 'HAC' AND IsAdmin = 0 AND ParentId IS NULL;
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
