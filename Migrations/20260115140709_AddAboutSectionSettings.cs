using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Umre.Web.Migrations
{
    /// <inheritdoc />
    public partial class AddAboutSectionSettings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
             migrationBuilder.Sql(@"
                IF NOT EXISTS (SELECT 1 FROM SiteAyarlari WHERE [Key] = 'About_Vision')
                BEGIN
                    INSERT INTO SiteAyarlari ([Key], [Value]) VALUES ('About_Vision', 'Sektörde öncü, yenilikçi ve güvenilir bir marka olarak, misafirlerimize en üst düzeyde hizmet sunmak ve manevi değerleri yaşatmak.')
                END

                IF NOT EXISTS (SELECT 1 FROM SiteAyarlari WHERE [Key] = 'About_Mission')
                BEGIN
                    INSERT INTO SiteAyarlari ([Key], [Value]) VALUES ('About_Mission', 'Misafir memnuniyetini esas alan anlayışımızla, tecrübemiz ve kalitemizle sektörde örnek gösterilen bir kurum olmak.')
                END

                IF NOT EXISTS (SELECT 1 FROM SiteAyarlari WHERE [Key] = 'About_Values')
                BEGIN
                    INSERT INTO SiteAyarlari ([Key], [Value]) VALUES ('About_Values', '<ul><li>Güvenilirlik</li><li>Kalite</li><li>Maneviyata Saygı</li><li>Müşteri Memnuniyeti</li></ul>')
                END
             ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
