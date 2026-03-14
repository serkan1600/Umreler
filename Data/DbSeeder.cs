using Microsoft.EntityFrameworkCore;
using Umre.Web.Models;
using System.Security.Cryptography;
using System.Text;

namespace Umre.Web.Data
{
    public static class DbSeeder
    {
        public static void Seed(AppDbContext context)
        {
            context.Database.Migrate();

            SeedAdmin(context);
            SeedAdmin(context);
            SeedSettings(context);
            CleanOldAdminMenus(context);
            CleanNewAdminMenus(context); 
            CleanupSpecificMenus(context);
            SeedMissingMenus(context);
            SeedPublicMenus(context); // ✅ Add this call
            
            context.SaveChanges();
        }

        private static void CleanupSpecificMenus(AppDbContext context)
        {
            var menusToClear = context.Menus.Where(m => m.Title == "Dil & Görünüm Ayarları" && m.IsAdmin).ToList();
            if (menusToClear.Any())
            {
                context.Menus.RemoveRange(menusToClear);
                context.SaveChanges();
            }
        }

        private static void CleanNewAdminMenus(AppDbContext context)
        {
            var title1 = "Tur Kategorileri";
            var title2 = "Görünüm Ayarları";
            var menus = context.Menus.Where(m => (m.Title == title1 || m.Title == title2) && m.IsAdmin).ToList();
            if (menus.Any())
            {
                context.Menus.RemoveRange(menus);
                context.SaveChanges();
            }
        }

        private static void CleanOldAdminMenus(AppDbContext context)
        {
            // Remove unwanted top-level menus that cause duplication
            var duplicates = context.Menus
                .Where(m => m.IsAdmin && (m.ParentId == null || m.ParentId == 0) && 
                           (m.Title.Contains("Hac Turları") || m.Title.Contains("Umre Turları")))
                .ToList();
            
            if (duplicates.Any())
            {
                context.Menus.RemoveRange(duplicates);
                context.SaveChanges();
            }
        }

        private static void SeedAdmin(AppDbContext context)
        {
            using var sha256 = SHA256.Create();
            var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes("1234"));
            var passwordHash = BitConverter.ToString(bytes).Replace("-", "").ToLower();

            var adminUser = context.AdminUsers.FirstOrDefault(u => u.Username == "admin");
            if (adminUser == null)
            {
                context.AdminUsers.Add(new AdminUser
                {
                    Username = "admin",
                    Password = passwordHash
                });
            }
            else
            {
                // Force reset password to ensure access
                adminUser.Password = passwordHash;
            }
            context.SaveChanges();
        }

        private static void SeedSettings(AppDbContext context)
        {
            var defaults = new Dictionary<string, string>
            {
                { "Brand_Name", "TEFEKKÜR TURİZM" },
                { "Contact_Phone", "0555 555 55 55" },
                { "Contact_Email", "info@tefekkurturizm.com" },
                { "Contact_Address", "Fatih, İstanbul / TÜRKİYE" },
                { "Module_Blog_Enabled", "true" },
                { "About_Title", "Hakkımızda" },
                { "About_Description", "Tefekkür Turizm, hac ve umre organizasyonlarında yılların vermiş olduğu tecrübeyle hizmet vermektedir." },
                { "About_Vision", "Hac ve Umre organizasyonlarında Türkiye’nin en güvenilir, en huzurlu, düzenli ve eksiksiz bir ibadet yapabilmeleri için tercih edilen marka şirketlerinden biri olmak." },
                { "About_Mission", "Misafirlerimize Hac ve Umre yolculuklarında manevi atmosferi en üst seviyede yaşatmak, rehberlik süreçlerini en yüksek kalitede yönetmek ve her adımda güvenilir bir yol arkadaşlığı sağlamaktır." },
                { "About_Values", "<ul><li>Güvenilirlik</li><li>Kalite</li><li>Maneviyat</li><li>Müşteri Memnuniyeti</li></ul>" },
                { "Social_Facebook", "https://facebook.com/" },
                { "Social_Instagram", "https://instagram.com/" },
                { "Social_Twitter", "https://twitter.com/" },
                { "Email_SmtpHost", "mail.site.com" },
                { "Email_SmtpPort", "587" },
                { "Email_Sender", "noreply@site.com" },
                { "Email_Password", "123456" },
                { "General_MarketingTitle", "Hacı Adaylarının İlk Tercihiyiz" },
                { "General_MarketingText", "Türkiye genelindeki acenteler arasında yıllarca hacı adaylarının ilk tercihi! Güven, huzur ve maneviyat odaklı hizmet anlayışımızla yanınızdayız." },
                { "LiveChat_Script", "<!-- Tawk.to Kodunuzu Buraya Yapıştırın -->" }
            };

            foreach (var kvp in defaults)
            {
                if (!context.SiteSettings.Any(x => x.Key == kvp.Key))
                {
                    context.SiteSettings.Add(new SiteSetting { Key = kvp.Key, Value = kvp.Value });
                }
            }
            context.SaveChanges();
        }

        // Keeping these methods just in case, but not calling them to comply with removal of seeding
        private static void SeedCategories(AppDbContext context) { }
        private static void SeedServices(AppDbContext context) { }
        private static void SeedPackages(AppDbContext context) { }
        private static void SeedMenus(AppDbContext context) { }



        private static void SeedMissingMenus(AppDbContext context)
        {
            // Helper to ensure parent exists and is active
            int EnsureParent(string title, string icon, int order)
            {
                var p = context.Menus.FirstOrDefault(m => m.Title == title && m.IsAdmin && (m.ParentId == null || m.ParentId == 0));
                if (p == null)
                {
                    p = new Menu { Title = title, Icon = icon, Url = "#", Order = order, IsActive = true, IsAdmin = true };
                    context.Menus.Add(p);
                    context.SaveChanges();
                }
                else if (!p.IsActive)
                {
                    // Ensure it is active if it exists
                    p.IsActive = true;
                    context.SaveChanges();
                }
                return p.Id;
            }

            // Helper to ensure specific child menu exists and is active
            void EnsureMenu(string title, string url, string icon, int parentId, int order)
            {
                var m = context.Menus.FirstOrDefault(x => x.Url == url && x.IsAdmin);
                if (m == null)
                {
                    context.Menus.Add(new Menu
                    {
                        Title = title,
                        Url = url,
                        Icon = icon,
                        ParentId = parentId,
                        Order = order,
                        IsActive = true,
                        IsAdmin = true
                    });
                }
                else
                {
                    // Update parent if changed (e.g. was orphan before) and ensure active
                    bool changed = false;
                    if (!m.IsActive) { m.IsActive = true; changed = true; }
                    if (m.ParentId != parentId) { m.ParentId = parentId; changed = true; }
                    if (changed) context.SaveChanges();
                }
            }

            // 1. İçerik Yönetimi
            var contentParentId = EnsureParent("İçerik Yönetimi", "fas fa-edit", 10);
            EnsureMenu("Sliderlar", "/Admin/Sliders", "fas fa-images", contentParentId, 1);
            EnsureMenu("Galeri", "/Admin/Gallery", "fas fa-camera", contentParentId, 2);
            EnsureMenu("Özellikler", "/Admin/Features", "fas fa-star", contentParentId, 3);
            EnsureMenu("Yorumlar", "/Admin/Testimonials", "fas fa-comments", contentParentId, 4);
            EnsureMenu("S.S.S.", "/Admin/Faqs", "fas fa-question-circle", contentParentId, 5);
            EnsureMenu("Hizmetler", "/Admin/Services", "fas fa-concierge-bell", contentParentId, 6);
            EnsureMenu("Blog Yönetimi", "/Admin/BlogPosts", "fas fa-blog", contentParentId, 7);

            // 2. Tur & Umre Yönetimi
            var tourParentId = EnsureParent("Tur & Rehber", "fas fa-globe", 20);
            EnsureMenu("Paketler", "/Admin/Packages", "fas fa-box-open", tourParentId, 1);
            EnsureMenu("Umre Rehberi", "/Admin/UmreGuides", "fas fa-book-open", tourParentId, 2);
            EnsureMenu("Umre Otelleri", "/Admin/UmreHotels", "fas fa-hotel", tourParentId, 3);
            EnsureMenu("Rehberler / Hocalar", "/Admin/Guides", "fas fa-user-tie", tourParentId, 4);

            // 3. Başvuru Yönetimi
            var formParentId = EnsureParent("Başvuru Yönetimi", "fas fa-file-signature", 30);
            EnsureMenu("Hac Ön Kayıtları", "/Admin/HacForms", "fas fa-kaaba", formParentId, 1);
            EnsureMenu("Umre Ön Kayıtları", "/Admin/UmreReservations", "fas fa-praying-hands", formParentId, 2);
            EnsureMenu("Tur Başvuruları", "/Admin/Bookings", "fas fa-file-contract", formParentId, 3);
            EnsureMenu("Sizi Arayalım", "/Admin/CallRequests", "fas fa-phone-volume", formParentId, 4);
            EnsureMenu("Değerlendirmeler", "/Admin/UmreForms", "fas fa-poll", formParentId, 5);

            // 4. İletişim
            var contactParentId = EnsureParent("İletişim", "fas fa-envelope", 40);
            EnsureMenu("Gelen Mesajlar", "/Admin/Messages", "fas fa-inbox", contactParentId, 1);
            EnsureMenu("Bülten Aboneleri", "/Admin/NewsletterSubscribers", "fas fa-mail-bulk", contactParentId, 2);

            // 5. Site Ayarları
            var settingsParentId = EnsureParent("Site Ayarları", "fas fa-cogs", 100);
            EnsureMenu("Genel Ayarlar", "/Admin/Settings", "fas fa-tools", settingsParentId, 1);
            EnsureMenu("Menü Yönetimi", "/Admin/Menus", "fas fa-bars", settingsParentId, 2);
            EnsureMenu("Popup Yönetimi", "/Admin/Popups", "fas fa-window-maximize", settingsParentId, 3);
            EnsureMenu("Geri Sayım", "/Admin/Countdown", "fas fa-clock", settingsParentId, 4);
            
            context.SaveChanges();
        }

        private static void SeedPublicMenus(AppDbContext context)
        {
            // Helper to ensure public menu exists
            void EnsurePublicMenu(string title, string url, int order, int? parentId = null)
            {
                var m = context.Menus.FirstOrDefault(x => x.Title == title && !x.IsAdmin);
                if (m == null)
                {
                    context.Menus.Add(new Menu
                    {
                        Title = title,
                        Url = url,
                        Order = order,
                        ParentId = parentId,
                        IsActive = true,
                        IsAdmin = false,
                        Icon = "fas fa-link" // Default icon
                    });
                }
                else
                {
                    // Update order and url if they don't match (enforcing structure)
                    bool changed = false;
                    if (m.Order != order) { m.Order = order; changed = true; }
                    
                    if (m.Url != url) { m.Url = url; changed = true; }
                    
                    // Force Active for Core Menus if requested to ensure they appear
                    if (!m.IsActive) { m.IsActive = true; changed = true; }

                    if (changed) context.SaveChanges();
                }
            }
            
            // 1. ANASAYFA
            EnsurePublicMenu("Anasayfa", "/", 1);

            // 2. HAKKIMIZDA
            EnsurePublicMenu("Hakkımızda", "/hakkimizda", 2);

            // 3. HAC (Parent)
            // Note: Hac ve Umre might be parents or direct links. Usually parents for dropdowns.
            // Let's check if they exist as categories. The user asked for "HAC" -> "HAC TURLARI" implies maybe a dropdown or just a title.
            // Assuming "HAC" is a direct link to Hac Turlari based on typical usage, or a dropdown header.
            // If it's a dropdown, URL can be empty or #.
            // For now, I'll map them as direct links if they don't have children yet.
            EnsurePublicMenu("Hac", "/hac-turlari", 3);

            // 4. UMRE
            EnsurePublicMenu("Umre", "/umre-turlari", 4);

            // 5. KÜLTÜR TURLARI
            EnsurePublicMenu("Kültür Turları", "/kultur-turlari", 5);

            // 6. HİZMETLER
            // Services usually have submenus. 
            EnsurePublicMenu("Hizmetler", "/#", 6);
            
            // 7. GALERİ
            EnsurePublicMenu("Galeri", "/galeri", 7);

            // 8. BLOGLAR
            EnsurePublicMenu("Bloglar", "/bloglar", 8);

            // 9. HOCALARIMIZ
            EnsurePublicMenu("Hocalarımız", "/hocalarimiz", 9);

            // 10. S.S.S
            EnsurePublicMenu("S.S.S", "/sikca-sorulan-sorular", 10);

            // 11. İLETİŞİM
            EnsurePublicMenu("İletişim", "/iletisim", 11);
            
            context.SaveChanges();
        }

    }
}
