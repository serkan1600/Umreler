using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Umre.Web.Migrations
{
    /// <inheritdoc />
    public partial class UpdateContentImportantInfo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                UPDATE Services 
                SET Content = '<h3>1. Pasaport ve Vize İşlemleri</h3><p>Umre veya Hac seyahatine çıkacak olan misafirlerimizin pasaport geçerlilik süresinin en az 1 yıl olması gerekmektedir. Çipli pasaport olması zorunludur. Vize işlemleri firmamız tarafından takip edilmekte olup, gerekli evraklar kayıt esnasında tarafınıza bildirilecektir.</p><h3>2. Sağlık ve Aşılar</h3><p>Suudi Arabistan Krallığı tarafından zorunlu tutulan <strong>Menenjit aşısı</strong>, seyahatten en az 10 gün önce yaptırılmalı ve aşı kartı yanınızda bulundurulmalıdır. Ayrıca düzenli kullandığınız ilaçlarınız varsa, raporları ile birlikte yanınıza almayı unutmayınız.</p><h3>3. İhram ve Kıyafet</h3><p>Erkekler için ihram, kadınlar için ise tesettüre uygun, rahat ve pamuklu kıyafetler tercih edilmelidir. Mekke ve Medine iklim şartları göz önünde bulundurularak, yanınıza güneş gözlüğü, terlik ve yürüyüş ayakkabısı almanız tavsiye edilir.</p><h3>4. Para ve İletişim</h3><p>Suudi Arabistan para birimi <strong>Riyal (SAR)</strong>''dir. Yanınızda bir miktar Riyal veya Dolar bulundurmanızda fayda vardır. İletişim için Türkiye''deki hattınızı yurtdışına açtırabilir veya orada uygun fiyatlı turist hatlarından temin edebilirsiniz.</p>'
                WHERE Slug = 'onemli-bilgiler'
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
