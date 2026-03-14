using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Umre.Web.Migrations
{
    /// <inheritdoc />
    public partial class AddAboutAdminMenu : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
             migrationBuilder.Sql(@"
                IF NOT EXISTS (SELECT 1 FROM Menuler WHERE Title = 'Hakkımızda' AND IsAdmin = 1)
                BEGIN
                    INSERT INTO Menuler (Title, Url, [Order], IsAdmin, ParentId, Icon, IsActive)
                    VALUES ('Hakkımızda', '/Admin/AboutSettings', 5, 1, NULL, 'fas fa-info-circle', 1)
                END
             ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
