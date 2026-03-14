using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Umre.Web.Migrations
{
    /// <inheritdoc />
    public partial class AddWhatsappMenu : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                DECLARE @ParentId INT = (SELECT TOP 1 Id FROM Menuler WHERE Title = 'Genel Ayarlar' AND IsAdmin = 1);
                
                IF @ParentId IS NOT NULL AND NOT EXISTS (SELECT 1 FROM Menuler WHERE Url = '/Admin/WhatsappSettings')
                BEGIN
                    INSERT INTO Menuler (Title, Url, [Order], IsActive, IsAdmin, ParentId, Icon)
                    VALUES ('WhatsApp Ayarları', '/Admin/WhatsappSettings', 8, 1, 1, @ParentId, NULL);
                END
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
