using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System.Security.Claims;
using Umre.Web.Data;
using Umre.Web.Models;

namespace Umre.Web.Controllers
{
    public class AdminController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly Umre.Web.Services.IEmailService _emailService;
        private readonly IMemoryCache _cache;

        public AdminController(AppDbContext context, IWebHostEnvironment hostEnvironment, Umre.Web.Services.IEmailService emailService, IMemoryCache cache)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
            _emailService = emailService;
            _cache = cache;
        }

        public static string HashPassword(string password)
        {
            using (var sha256 = System.Security.Cryptography.SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
            }
        }

        // Helper for CSV
        private string ToCsv<T>(IEnumerable<T> data)
        {
            var properties = typeof(T).GetProperties()
                .Where(p => p.PropertyType == typeof(string)
                         || p.PropertyType == typeof(int)
                         || p.PropertyType == typeof(DateTime)
                         || p.PropertyType == typeof(bool)
                         || p.PropertyType == typeof(decimal));

            var csv = new System.Text.StringBuilder();

            // Header
            csv.AppendLine(string.Join(",", properties.Select(p => p.Name)));

            // Rows
            foreach (var item in data)
            {
                var values = properties.Select(p =>
                {
                    var val = p.GetValue(item);
                    var str = val?.ToString() ?? "";

                    if (str.Contains(",") || str.Contains("\"") || str.Contains("\n") || str.Contains("\r"))
                    {
                        str = "\"" + str.Replace("\"", "\"\"") + "\"";
                    }
                    return str;
                });

                csv.AppendLine(string.Join(",", values));
            }

            return csv.ToString();
        }

        private bool IsImageFileAllowed(IFormFile file)
        {
            if (file == null) return true;
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
            var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
            return allowedExtensions.Contains(ext);
        }

        private bool IsGalleryFileAllowed(IFormFile file)
        {
            if (file == null) return true;
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp", ".mp4", ".mov", ".avi", ".mkv", ".webm" };
            var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
            return allowedExtensions.Contains(ext);
        }

        // ==============================
        // LOGIN
        // ==============================
        [HttpGet]
        public IActionResult Login()
        {
            if (User.Identity!.IsAuthenticated)
            {
                return RedirectToAction("Index");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            var passwordHash = HashPassword(password);

            var admin = await _context.AdminUsers
                .FirstOrDefaultAsync(u => u.Username == username && u.Password == passwordHash);

            if (admin != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, admin.Username),
                    new Claim(ClaimTypes.Role, "Admin")
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties();

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties);

                return RedirectToAction("Index");
            }

            ViewBag.Error = "Kullanıcı adı veya şifre hatalı.";
            return View();
        }

        // ✅ DÜZELTİLMİŞ ŞİFREMİ UNUTTUM
        [HttpPost]
        public async Task<IActionResult> ForgotPassword(string username, string securityCode)
        {
            if (string.IsNullOrWhiteSpace(username))
            {
                TempData["Error"] = "Kullanıcı adı boş olamaz.";
                return RedirectToAction("Login");
            }

            username = username.Trim();

            var allSettings = await _context.SiteSettings.ToListAsync();
            var resetCode = allSettings.FirstOrDefault(s => s.Key == "Security_ResetCode")?.Value;
            if (string.IsNullOrEmpty(resetCode)) resetCode = "1453";

            // Güvenlik kodu doğruysa şifreyi 1234 yap
            if (!string.IsNullOrEmpty(securityCode) && securityCode == resetCode)
            {
                var adminUser = await _context.AdminUsers.FirstOrDefaultAsync(u => u.Username.ToLower() == username.ToLower());
                if (adminUser != null)
                {
                    adminUser.Password = HashPassword("1234");
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Şifreniz başarıyla '1234' olarak sıfırlandı. Lütfen giriş yapınız.";
                    return RedirectToAction("Login");
                }

                TempData["Error"] = $"'{username}' kullanıcı adına sahip bir yönetici bulunamadı.";
                return RedirectToAction("Login");
            }

            // Admini bul
            AdminUser admin = await _context.AdminUsers
                .FirstOrDefaultAsync(u => u.Username.ToLower() == username.ToLower());

            if (admin == null)
            {
                TempData["Error"] = $"'{username}' kullanıcı adına sahip bir yönetici bulunamadı.";
                return RedirectToAction("Login");
            }

            // SMTP ayarları
            var host = allSettings.FirstOrDefault(s => s.Key == "Mail_Host")?.Value;
            var portStr = allSettings.FirstOrDefault(s => s.Key == "Mail_Port")?.Value;
            var user = allSettings.FirstOrDefault(s => s.Key == "Mail_User")?.Value;
            var pass = allSettings.FirstOrDefault(s => s.Key == "Mail_Password")?.Value;

            if (string.IsNullOrEmpty(host) || string.IsNullOrEmpty(user) || string.IsNullOrEmpty(pass))
            {
                TempData["Error"] = "Mail gönderimi başarısız (Sunucu Ayarları Eksik). Kurtarma kodu ile sıfırlayınız.";
                return RedirectToAction("Login");
            }

            // Yeni şifre üret
            var newPassword = Guid.NewGuid().ToString().Substring(0, 8);
            admin.Password = HashPassword(newPassword);
            await _context.SaveChangesAsync();

            var recipient = allSettings.FirstOrDefault(s => s.Key == "Mail_ResetRecipient")?.Value;
            if (string.IsNullOrEmpty(recipient)) recipient = user;

            string subject = "Admin Panel Şifre Sıfırlama";
            string body = $@"
                <div style='font-family: Arial, sans-serif; padding: 20px; border: 1px solid #eee; border-radius: 5px;'>
                    <h2 style='color: #d4af37;'>Şifre Sıfırlama</h2>
                    <p>Admin paneli giriş şifreniz sıfırlanmıştır.</p>
                    <p><strong>Yeni Şifreniz:</strong> {newPassword}</p>
                    <hr>
                    <small style='color: #999;'>Bu işlem talebiniz üzerine gerçekleştirilmiştir.</small>
                </div>
            ";

            if (!int.TryParse(portStr, out int port))
            {
                TempData["Error"] = "Mail port ayarı hatalı.";
                return RedirectToAction("Login");
            }

            try
            {
                await _emailService.SendEmailAsync(host, port, user, pass, recipient, subject, body);
                TempData["Success"] = "Yeni şifreniz sistemde kayıtlı e-posta adresine gönderildi.";
                return RedirectToAction("Login");
            }
            catch (Exception ex)
            {
                var errorMsg = ex.Message;
                if (ex.InnerException != null) errorMsg += " (" + ex.InnerException.Message + ")";
                TempData["Error"] = $"Mail gönderimi başarısız oldu: {errorMsg}";
                return RedirectToAction("Login");
            }
        }

        // ==============================
        // LOGOUT
        // ==============================
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }

        // ==============================
        // ADMIN PANEL
        // ==============================
        [Authorize]
        public async Task<IActionResult> Index()
        {
            ViewBag.PackageCount = await _context.Packages.CountAsync();
            ViewBag.BookingCount = await _context.Bookings.CountAsync(b => !b.IsDeleted);
            ViewBag.NewsletterCount = await _context.NewsletterSubscribers.CountAsync();
            ViewBag.BlogCount = await _context.BlogPosts.CountAsync();
            ViewBag.TestimonialCount = await _context.Testimonials.CountAsync();
            ViewBag.MessageCount = await _context.ContactMessages.CountAsync(m => !m.IsRead);
            ViewBag.HacFormCount = await _context.HacRegistrations.CountAsync(h => !h.IsRead);
            ViewBag.UmreFormCount = await _context.UmreReservations.CountAsync(u => !u.IsRead && !u.IsDeleted);
            ViewBag.CallReqCount = await _context.CallRequests.CountAsync(c => !c.IsProcessed && !c.IsDeleted);
            ViewBag.SurveyCount = await _context.UmreForms.CountAsync(u => !u.IsRead);
            ViewBag.GalleryCount = await _context.GalleryItems.CountAsync();
            ViewBag.SliderCount = await _context.Sliders.CountAsync();

            
            // --- Chart Data Preparation ---

            // 1. Line Chart: Last 6 Months Bookings
            var sixMonthsAgo = DateTime.Now.AddMonths(-6);
            var bookingStats = await _context.Bookings
                .Where(b => b.CreatedAt >= sixMonthsAgo && !b.IsDeleted)
                .GroupBy(b => new { b.CreatedAt.Year, b.CreatedAt.Month })
                .Select(g => new { Date = new DateTime(g.Key.Year, g.Key.Month, 1), Count = g.Count() })
                .ToListAsync();

            var lineLabels = new List<string>();
            var lineData = new List<int>();

            for (int i = 5; i >= 0; i--)
            {
                var date = DateTime.Now.AddMonths(-i);
                var stat = bookingStats.FirstOrDefault(s => s.Date.Month == date.Month && s.Date.Year == date.Year);
                lineLabels.Add(date.ToString("MMMM"));
                lineData.Add(stat?.Count ?? 0);
            }

            ViewBag.ChartLabels = Newtonsoft.Json.JsonConvert.SerializeObject(lineLabels);
            ViewBag.ChartData = Newtonsoft.Json.JsonConvert.SerializeObject(lineData);


            // 2. Pie Chart: Top Packages
            var topPackages = await _context.Bookings
                .Include(b => b.Package)
                .Where(b => !b.IsDeleted && b.PackageId != null)
                .GroupBy(b => b.Package.Title)
                .Select(g => new { Title = g.Key, Count = g.Count() })
                .OrderByDescending(x => x.Count)
                .Take(5)
                .ToListAsync();

            ViewBag.PieLabels = Newtonsoft.Json.JsonConvert.SerializeObject(topPackages.Select(x => x.Title));
            ViewBag.PieData = Newtonsoft.Json.JsonConvert.SerializeObject(topPackages.Select(x => x.Count));

            // 3. Recent Activity Lists (for Index View)
            ViewBag.RecentBookings = await _context.Bookings.Include(b => b.Package).Where(b => !b.IsDeleted).OrderByDescending(b => b.CreatedAt).Take(5).ToListAsync();
            ViewBag.RecentCallRequests = await _context.CallRequests.OrderByDescending(c => c.CreatedAt).Take(5).ToListAsync();


            return View();
        }

        // ==============================
        // EXPORT ACTIONS
        // ==============================
        [Authorize(Roles = "Admin")]
        [Route("/Admin/ExportBookings")]
        public async Task<IActionResult> ExportBookings()
        {
            var data = await _context.Bookings
                .Include(b => b.Package)
                .Where(b => !b.IsDeleted)
                .OrderByDescending(b => b.CreatedAt)
                .Select(b => new 
                {
                    Tarih = b.CreatedAt.ToString("dd.MM.yyyy HH:mm"),
                    Musteri = b.CustomerName,
                    Telefon = b.Phone,
                    Email = b.Email,
                    Tur = b.Package != null ? b.Package.Title : "Silinmiş",
                    Not = b.Note,
                    Durum = b.Status
                })
                .ToListAsync();

            var csv = ToCsv(data);
            var bytes = System.Text.Encoding.UTF8.GetBytes(csv);
            // Add BOM for Excel
            var result = System.Text.Encoding.UTF8.GetPreamble().Concat(bytes).ToArray();

            return File(result, "text/csv", $"basvurular_{DateTime.Now:ddMMyyyy}.csv");
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [Route("/Admin/BackupDatabase")]
        public async Task<IActionResult> BackupDatabase()
        {
            var data = new
            {
                Packages = await _context.Packages.AsNoTracking().ToListAsync(),
                BlogPosts = await _context.BlogPosts.AsNoTracking().ToListAsync(),
                Bookings = await _context.Bookings.AsNoTracking().ToListAsync(),
                Testimonials = await _context.Testimonials.AsNoTracking().ToListAsync(),
                Faqs = await _context.Faqs.AsNoTracking().ToListAsync(),
                Menus = await _context.Menus.AsNoTracking().ToListAsync(),
                SiteSettings = await _context.SiteSettings.AsNoTracking().ToListAsync(),
                Features = await _context.Features.AsNoTracking().ToListAsync(),
                HacRegistrations = await _context.HacRegistrations.AsNoTracking().ToListAsync(),
                UmreReservations = await _context.UmreReservations.AsNoTracking().ToListAsync(),
                GalleryItems = await _context.GalleryItems.AsNoTracking().ToListAsync(),
                Sliders = await _context.Sliders.AsNoTracking().ToListAsync(),
                CallRequests = await _context.CallRequests.AsNoTracking().ToListAsync()
            };

            var json = Newtonsoft.Json.JsonConvert.SerializeObject(data, Newtonsoft.Json.Formatting.Indented);
            var bytes = System.Text.Encoding.UTF8.GetBytes(json);

            return File(bytes, "application/json", $"backup_{DateTime.Now:yyyyMMdd_HHmm}.json");
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Menus()
        {
            var menus = _context.Menus
                .OrderBy(m => m.Order)
                .ToList();

            return View(menus);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Settings()
        {
            var settings = await _context.SiteSettings.ToListAsync();
            return View(settings);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateSettings(Dictionary<string, string> settings)
        {
            foreach (var item in settings)
            {
                var val = item.Value ?? "";
                if (val.Contains(",")) val = val.Split(',')[0]; // Take "true" from "true,false"

                var s = await _context.SiteSettings.FirstOrDefaultAsync(x => x.Key == item.Key);
                if (s == null)
                {
                    _context.SiteSettings.Add(new SiteSetting { Key = item.Key, Value = val });
                }
                else
                {
                    s.Value = val;
                    _context.SiteSettings.Update(s);
                }
            }
            await _context.SaveChangesAsync();
            _cache.Remove("SiteSettings");
            TempData["Success"] = "Ayarlar başarıyla güncellendi.";
            return RedirectToAction(nameof(Settings));
        }

        // Toggle active status of a menu item (AJAX friendly)
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ToggleMenu(int id)
        {
            var menu = await _context.Menus.FindAsync(id);
            if (menu == null)
            {
                // Return JSON for AJAX, also handle non‑AJAX fallback
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                    return Json(new { success = false, message = "Menu not found." });
                return NotFound();
            }

            menu.IsActive = !menu.IsActive;
            await _context.SaveChangesAsync();

            // If request is AJAX, return JSON, otherwise redirect back to Menus page
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return Json(new { success = true, isActive = menu.IsActive, message = "Status updated." });
            }
            return RedirectToAction(nameof(Menus));
        }

    }
}
