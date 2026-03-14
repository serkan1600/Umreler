using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Umre.Web.Migrations
{
    /// <inheritdoc />
    public partial class RemoveUmrePageMenus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Delete sub-menus and the parent menu 'Umre Sayfa Yönetimi'
            // We use SQL to be precise.
            migrationBuilder.Sql("DELETE FROM Menus WHERE Title IN ('Umre Rehberi', 'Oteller', 'Umre Otelleri', 'Umre Eğitimleri', 'Kayıt Formları')");
            migrationBuilder.Sql("DELETE FROM Menus WHERE Title = 'Umre Sayfa Yönetimi'");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
