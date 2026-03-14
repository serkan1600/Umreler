using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Umre.Web.Migrations
{
    /// <inheritdoc />
    public partial class SyncAdminModulesSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                -- Find Parent ID for 'Genel Ayarlar'
                DECLARE @GeneralSettingsId INT = (SELECT Top 1 Id FROM Menuler WHERE Title LIKE '%Genel Ayarlar%' AND IsAdmin = 1);
                IF @GeneralSettingsId IS NULL
                BEGIN
                    INSERT INTO Menuler (Title, Url, [Order], IsActive, IsAdmin, Icon) VALUES ('Genel Ayarlar', '#', 90, 1, 1, 'fas fa-cogs');
                    SET @GeneralSettingsId = SCOPE_IDENTITY();
                END

                -- Hakkımızda Ayarları
                IF NOT EXISTS (SELECT 1 FROM Menuler WHERE Url = '/Admin/AboutSettings')
                BEGIN
                    INSERT INTO Menuler (Title, Url, [Order], IsActive, IsAdmin, ParentId, Icon)
                    VALUES ('Hakkımızda Ayarları', '/Admin/AboutSettings', 3, 1, 1, @GeneralSettingsId, 'fas fa-info-circle');
                END

                -- Güvenlik Ayarları
                IF NOT EXISTS (SELECT 1 FROM Menuler WHERE Url = '/Admin/SecuritySettings')
                BEGIN
                    INSERT INTO Menuler (Title, Url, [Order], IsActive, IsAdmin, ParentId, Icon)
                    VALUES ('Güvenlik Ayarları', '/Admin/SecuritySettings', 4, 1, 1, @GeneralSettingsId, 'fas fa-lock');
                END

                -- Galeri URL Fix (Ensure it matches our new Controller)
                UPDATE Menuler SET Url = '/Admin/Gallery' WHERE Url LIKE '%/Admin/GalleryItems%';

                -- Countdown URL Fix
                UPDATE Menuler SET Url = '/Admin/Countdown' WHERE Url LIKE '%/Admin/Countdowns%';
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
