using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Umre.Web.Migrations
{
    /// <inheritdoc />
    public partial class AddServicesData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
             migrationBuilder.Sql(@"
                IF NOT EXISTS (SELECT 1 FROM Services WHERE Slug = 'banka-hesaplari')
                BEGIN
                    INSERT INTO Services (Title, Slug, [Content], IsActive, [Order])
                    VALUES ('Banka Hesap Numaraları', 'banka-hesaplari', '<h3>Banka Hesaplarımız</h3><p>Lütfen ödeme yaparken açıklama kısmına isminizi yazmayı unutmayınız.</p><p><strong>Ziraat Bankası:</strong> TR00 0000 0000 0000<br><strong>Kuveyt Türk:</strong> TR00 0000 0000 0000</p>', 1, 0)
                END

                IF EXISTS (SELECT 1 FROM Services WHERE Slug = 'onemli-bilgiler')
                BEGIN
                    UPDATE Services SET Title = 'Önemli Bilgiler ve Uyarılar' WHERE Slug = 'onemli-bilgiler'
                END
             ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
