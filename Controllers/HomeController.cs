using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Umre.Web.Data;
using Umre.Web.Models;
using Umre.Web.ViewModels;


namespace Umre.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;
        private readonly Services.IEmailService _emailService;
        private readonly IWebHostEnvironment _hostEnvironment;

        public HomeController(AppDbContext context, Services.IEmailService emailService, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _emailService = emailService;
            _hostEnvironment = hostEnvironment;
        }

        public async Task<IActionResult> Index()
        {
            var packages = await _context.Packages.AsNoTracking()
                .Include(p => p.Category)
                .Where(p => p.IsActive && 
                            p.Category != null && 
                            (p.Category.Name.Contains("Hac") || p.Category.Name.Contains("Umre") || p.Title.Contains("Hac") || p.Title.Contains("Umre")))
                .ToListAsync();
            var sliders = await _context.Sliders.AsNoTracking().Where(s => s.IsActive).OrderBy(s => s.SortOrder).ToListAsync();
            var blogs = await _context.BlogPosts.AsNoTracking().Where(b => b.IsActive).OrderByDescending(b => b.PublishedDate).Take(3).ToListAsync();
            var testimonials = await _context.Testimonials.AsNoTracking().Where(t => t.IsActive).OrderBy(t => t.Order).ToListAsync();
            var features = await _context.Features.AsNoTracking().Where(f => f.IsActive && (f.Group == "HomePage" || f.Group == "General" || f.Group == null)).OrderBy(f => f.Order).ToListAsync();
            var countdown = await _context.Countdowns.AsNoTracking().FirstOrDefaultAsync(c => c.IsActive && c.TargetDate > DateTime.Now);
            

            var nextTour = await _context.Packages.AsNoTracking()
                .Where(p => p.IsActive && p.StartDate > DateTime.Now)
                .OrderBy(p => p.StartDate)
                .FirstOrDefaultAsync();

            if (nextTour == null)
            {
                nextTour = new Package 
                { 
                    Id = 0, 
                    Title = "2026 Şevval Umresi (Yakında)", 
                    StartDate = DateTime.Now.AddDays(15).AddHours(4),
                    Description = "Demo Tur"
                };
            }

            var model = new HomeViewModel
            {
                Packages = packages,
                Sliders = sliders,
                RecentBlogs = blogs,
                Testimonials = testimonials,
                Features = features,
                NextTour = nextTour,
                Countdown = countdown
            };
            
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Subscribe(string email)
        {
            if (string.IsNullOrEmpty(email) || !email.Contains("@"))
            {
                 return Json(new { success = false, message = "Lütfen geçerli bir e-posta adresi giriniz." });
            }

            var existing = await _context.NewsletterSubscribers.FirstOrDefaultAsync(s => s.Email == email);
            if (existing != null)
            {
                 return Json(new { success = true, message = "Bu e-posta adresi zaten kayıtlıdır." });
            }

            var subscriber = new NewsletterSubscriber
            {
                Email = email,
                SubscribedAt = DateTime.Now,
                IsActive = true
            };

            try {
                _context.NewsletterSubscribers.Add(subscriber);
                await _context.SaveChangesAsync();
                return Json(new { success = true, message = "Bültenimize başarıyla abone oldunuz." });
            }
            catch
            {
                return Json(new { success = false, message = "Bir hata oluştu. Lütfen daha sonra tekrar deneyiniz." });
            }
        }


        [Route("/banka-hesaplari")]
        [Route("/hesap-numaralari")]
        public async Task<IActionResult> BankAccounts()
        {
             var service = await _context.Services.FirstOrDefaultAsync(s => s.Slug == "banka-hesaplari" && s.IsActive);
             if(service == null) return NotFound();
             return View("ServiceDetails", service);
        }

        [Route("/onemli-bilgiler")]
        [Route("/bilgi-bankasi")]
        public async Task<IActionResult> ImportantInfo()
        {
             var service = await _context.Services.FirstOrDefaultAsync(s => s.Slug == "onemli-bilgiler" && s.IsActive);
             if(service == null) return NotFound();
             return View("ServiceDetails", service);
        }

        [Route("hizmetler/{slug}")]
        public async Task<IActionResult> ServiceDetails(string slug)
        {
             var service = await _context.Services.FirstOrDefaultAsync(s => s.Slug == slug && s.IsActive);
             
             
             if(service == null)
             {
                 if(slug == "onemli-bilgiler" || slug == "bilgi-bankasi") return RedirectToAction("ImportantInfo");
                 if(slug == "banka-hesaplari") return RedirectToAction("BankAccounts");
                 return NotFound();
             }

             return View(service);
        }


        [Route("tur-detay/{id}")]
        [Route("paket-detay/{id}")]
        [Route("Home/Details/{id}")]
        public async Task<IActionResult> Details(int id)
        {
            var package = await _context.Packages
                .Include(p => p.TourFlows)
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (package == null) return NotFound();

            var waSetting = await _context.SiteSettings.FirstOrDefaultAsync(s => s.Key == "Contact_Whatsapp");
            ViewBag.WhatsappNumber = !string.IsNullOrEmpty(waSetting?.Value) ? waSetting.Value : "905454836943";

            return View(package);
        }

        [Route("rehberlerimiz")]
        [Route("hocalarimiz")]
        public async Task<IActionResult> Guides()
        {
            var guides = await _context.Guides.Where(g => g.IsActive).OrderBy(g => g.Order).ToListAsync();
            return View(guides);
        }



        [Route("galeri")]
        public async Task<IActionResult> Gallery()
        {
            var items = await _context.GalleryItems.Where(g => g.IsActive).OrderByDescending(g => g.Id).ToListAsync();
            return View(items);
        }

        [Route("blog/{id}")]
        [Route("haber/{id}")]
        [Route("Home/BlogDetails/{id}")]
        public async Task<IActionResult> BlogDetails(int id)
        {
            var blog = await _context.BlogPosts.FindAsync(id);
            if (blog == null) return NotFound();


            ViewBag.RecentBlogs = await _context.BlogPosts
                                    .Where(b => b.IsActive && b.Id != id)
                                    .OrderByDescending(b => b.PublishedDate)
                                    .Take(5)
                                    .ToListAsync();

            return View(blog);
        }

        [Route("sikca-sorulan-sorular")]
        [Route("Home/Faq")]
        public async Task<IActionResult> Faq()
        {
            var faqs = await _context.Faqs.Where(f => f.IsActive).OrderBy(f => f.Order).ToListAsync();
            return View(faqs);
        }

        [Route("iletisim")]
        [Route("Home/Contact")]
        public async Task<IActionResult> Contact()
        {
             var settings = await _context.SiteSettings.ToListAsync();
             ViewBag.Address = settings.FirstOrDefault(s => s.Key == "Contact_Address")?.Value;
             ViewBag.Phone = settings.FirstOrDefault(s => s.Key == "Contact_Phone")?.Value;
             ViewBag.Email = settings.FirstOrDefault(s => s.Key == "Contact_Email")?.Value;
             ViewBag.MapUrl = settings.FirstOrDefault(s => s.Key == "Contact_MapUrl")?.Value;
             return View();
        }

        [HttpPost]
        [Route("iletisim")]
        [Route("Home/Contact")]
        public async Task<IActionResult> Contact(ContactMessage model)
        {
            if (ModelState.IsValid)
            {
                model.CreatedAt = DateTime.Now;
                _context.ContactMessages.Add(model);
                await _context.SaveChangesAsync();
                
                 // Notifications
                 await SendAdminNotification("Yeni İletişim Mesajı", $"<p><strong>İsim:</strong> {model.Name}</p><p><strong>Konu:</strong> {model.Subject}</p><p><strong>Mesaj:</strong> {model.Message}</p>");
                
                if(!string.IsNullOrEmpty(model.Email))
                {
                     await SendUserConfirmation(model.Email, "Mesajınız Alındı", 
                         $@"<h3>Sayın {model.Name},</h3>
                            <p>Gönderdiğiniz mesaj tarafımıza ulaşmıştır. En kısa sürede dönüş yapılacaktır.</p>
                            <br>
                            <p>Saygılarımızla,<br>TEFEKKÜR TURİZM</p>");
                }

                TempData["Success"] = "Mesajınız başarıyla gönderildi. En kısa sürede sizinle iletişime geçeceğiz.";
                return RedirectToAction("Contact");
            }
            
             var settings = await _context.SiteSettings.ToListAsync();
             ViewBag.Address = settings.FirstOrDefault(s => s.Key == "Contact_Address")?.Value;
             ViewBag.Phone = settings.FirstOrDefault(s => s.Key == "Contact_Phone")?.Value;
             ViewBag.Email = settings.FirstOrDefault(s => s.Key == "Contact_Email")?.Value;
             ViewBag.MapUrl = settings.FirstOrDefault(s => s.Key == "Contact_MapUrl")?.Value;
             
             TempData["Error"] = "Lütfen tüm alanları doldurunuz.";
            return View(model);
        }



        [Route("hac-turlari")]
        [Route("kategori/hac-turlari")]
        [Route("Home/HacTurlari")]
        public async Task<IActionResult> HacTurlari(decimal? minPrice, decimal? maxPrice, DateTime? filterStartDate, DateTime? filterEndDate, string sortOrder)
        {

            var query = _context.Packages.AsQueryable().Where(p => p.IsActive && p.Title.Contains("Hac"));

            if (minPrice.HasValue) query = query.Where(p => p.Price >= minPrice.Value);
            if (maxPrice.HasValue) query = query.Where(p => p.Price <= maxPrice.Value);
            if (filterStartDate.HasValue) query = query.Where(p => p.StartDate >= filterStartDate.Value);
            if (filterEndDate.HasValue) query = query.Where(p => p.EndDate <= filterEndDate.Value);

             switch (sortOrder)
            {
                case "price_asc": query = query.OrderBy(p => p.Price); break;
                case "price_desc": query = query.OrderByDescending(p => p.Price); break;
                case "date_desc": query = query.OrderByDescending(p => p.StartDate); break;
                default: query = query.OrderBy(p => p.StartDate); break;
            }

            var packages = await query.ToListAsync();

            ViewBag.MinPrice = minPrice;
            ViewBag.MaxPrice = maxPrice;
            ViewBag.StartDate = filterStartDate?.ToString("yyyy-MM-dd");
            ViewBag.EndDate = filterEndDate?.ToString("yyyy-MM-dd");
            ViewBag.SortOrder = sortOrder;
            
            ViewBag.PageTitle = "Hac Turları";
            
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_PackageListPartial", packages);
            }
            
            return View(packages);
        }

        [Route("umre-turlari")]
        [Route("kategori/umre-turlari")]
        [Route("Home/UmreTurlari")]
        public async Task<IActionResult> UmreTurlari(decimal? minPrice, decimal? maxPrice, DateTime? filterStartDate, DateTime? filterEndDate, string sortOrder)
        {

            var query = _context.Packages.AsQueryable().Where(p => p.IsActive && p.Title.Contains("Umre"));
            
            var allQuery = query;
            
            if (minPrice.HasValue) allQuery = allQuery.Where(p => p.Price >= minPrice.Value);
            if (maxPrice.HasValue) allQuery = allQuery.Where(p => p.Price <= maxPrice.Value);
            if (filterStartDate.HasValue) allQuery = allQuery.Where(p => p.StartDate >= filterStartDate.Value);
            if (filterEndDate.HasValue) allQuery = allQuery.Where(p => p.EndDate <= filterEndDate.Value);

            switch (sortOrder)
            {
                case "price_asc": allQuery = allQuery.OrderBy(p => p.Price); break;
                case "price_desc": allQuery = allQuery.OrderByDescending(p => p.Price); break;
                case "date_desc": allQuery = allQuery.OrderByDescending(p => p.StartDate); break;
                default: allQuery = allQuery.OrderBy(p => p.StartDate); break;
            }

            var packages = await allQuery.ToListAsync();
            
             ViewBag.MinPrice = minPrice;
            ViewBag.MaxPrice = maxPrice;
            ViewBag.StartDate = filterStartDate?.ToString("yyyy-MM-dd");
            ViewBag.EndDate = filterEndDate?.ToString("yyyy-MM-dd");
            ViewBag.SortOrder = sortOrder;
            ViewBag.PageTitle = "Umre Turları";
            ViewBag.ActionName = "UmreTurlari";

            return View(packages);
        }



        [HttpGet]
        [Route("hac-formu")]
        [Route("hac-on-kayit")]
        [Route("Home/HacFormu")]
        public IActionResult HacFormu()
        {
            return View();
        }

        [HttpPost]
        [Route("hac-formu")]
        [Route("hac-on-kayit")]
        [Route("Home/HacFormu")]
        public async Task<IActionResult> HacFormu(HacRegistration model)
        {
            if (ModelState.IsValid)
            {
                model.CreatedAt = DateTime.Now;
                _context.HacRegistrations.Add(model);
                await _context.SaveChangesAsync();
                
                // Notifications
                await SendAdminNotification("Yeni Hac Ön Kayıt", $"<p><strong>İsim:</strong> {model.FullName}</p><p><strong>Telefon:</strong> {model.Phone}</p><p><strong>E-Posta:</strong> {model.Email}</p><p><strong>Şehir:</strong> {model.City}</p>");
                
                if(!string.IsNullOrEmpty(model.Email))
                {
                     await SendUserConfirmation(model.Email, "Hac Ön Kayıt Başvurunuz Alındı", 
                         $@"<h3>Sayın {model.FullName},</h3>
                            <p>Hac ön kayıt başvurunuzu aldık. İlginiz için teşekkür ederiz.</p>
                            <p>Uzman ekibimiz başvurunuzu inceleyip en kısa sürede sizinle iletişime geçecektir.</p>
                            <br>
                            <p>Saygılarımızla,<br>TEFEKKÜR TURİZM</p>");
                }

                TempData["Success"] = "Hac ön kayıt talebiniz alınmıştır. En kısa sürede sizinle iletişime geçilecektir.";
                return RedirectToAction("HacFormu");
            }
            return View(model);
        }



        [HttpGet]
        [Route("umre-rehberi")]
        public async Task<IActionResult> UmreRehberi()
        {
            var guides = await _context.UmreGuides.Where(g => g.IsActive).ToListAsync();
            

            var settings = await _context.SiteSettings.Where(s => s.Key.StartsWith("UmreGuides_")).ToListAsync();
            ViewBag.Settings = settings;


            ViewBag.RecentTours = await _context.Packages.OrderByDescending(p => p.Id).Take(5).ToListAsync();

            return View(guides);
        }

        [Route("umre-otellerimiz")]
        public async Task<IActionResult> UmreOtellerimiz()
        {
            var hotels = await _context.UmreHotels.Where(h => h.IsActive).ToListAsync();
            var settings = await _context.SiteSettings.Where(s => s.Key.StartsWith("UmreHotels_")).ToListAsync();
            ViewBag.Settings = settings;
            return View(hotels);
        }
        
        [Route("umre-egitimi")]
        [Route("umre-seminerleri")]
        public async Task<IActionResult> UmreEgitimi()
        {
            var educations = await _context.UmreEducations.Where(e => e.IsActive).OrderBy(e => e.EventDate).ToListAsync();
            return View(educations);
        }

        [HttpGet]
        [Route("umre-anketi")]
        [Route("degerlendirme-anketi")]
        [Route("Home/UmreAnketi")]
        public async Task<IActionResult> UmreAnketi()
        {

            ViewBag.RecentTours = await _context.Packages.OrderByDescending(p => p.Id).Take(5).ToListAsync();
            return View();
        }

        [HttpPost]
        [Route("umre-anketi")]
        [Route("degerlendirme-anketi")]
        [Route("Home/UmreAnketi")]
        public async Task<IActionResult> UmreAnketi(UmreForm model)
        {
            if (ModelState.IsValid)
            {
                model.CreatedAt = DateTime.Now;
                _context.UmreForms.Add(model);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Ankebiniz başarıyla gönderildi. Teşekkür ederiz.";
                return RedirectToAction("UmreAnketi");
            }
             TempData["Error"] = "Lütfen tüm alanları doldurunuz.";
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Book(int PackageId, string CustomerName, string Phone, string Email, string Note, string PersonCount, string RoomType)
        {
            var fullNote = Note;
            if(!string.IsNullOrEmpty(PersonCount) || !string.IsNullOrEmpty(RoomType))
            {
                fullNote = $"[Sihirbaz] Kişi Sayısı: {PersonCount}, Oda Tipi: {RoomType} | Not: {Note}";
            }

            var booking = new Booking
            {
                PackageId = PackageId,
                CustomerName = CustomerName,
                Phone = Phone,
                Email = Email,
                Note = fullNote,
                CreatedAt = DateTime.Now
            };
            
            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Başvurunuz başarıyla alınmıştır. Teşekkür ederiz.";
            return RedirectToAction("Details", new { id = PackageId });
        }

        [HttpGet]
        [Route("umre-formu")]
        [Route("umre-kayit")]
        [Route("Home/UmreFormu")]
        public IActionResult UmreFormu()
        {
            return View();
        }

        [HttpPost]
        [Route("umre-formu")]
        [Route("umre-kayit")]
        [Route("Home/UmreFormu")]
        public async Task<IActionResult> UmreFormu(Umre.Web.Models.UmreReservation model)
        {
            if (ModelState.IsValid)
            {
                model.CreatedAt = DateTime.Now;
                _context.UmreReservations.Add(model);
                await _context.SaveChangesAsync();

                // Notifications
                 await SendAdminNotification("Yeni Umre Rezervasyonu", $"<p><strong>İsim:</strong> {model.FullName}</p><p><strong>Telefon:</strong> {model.Phone}</p><p><strong>E-Posta:</strong> {model.Email}</p><p><strong>Tarih:</strong> {model.PreferredDate}</p>");
                
                if(!string.IsNullOrEmpty(model.Email))
                {
                     await SendUserConfirmation(model.Email, "Umre Rezervasyon Talebiniz Alındı", 
                         $@"<h3>Sayın {model.FullName},</h3>
                            <p>Umre kayıt talebiniz tarafımıza ulaşmıştır.</p>
                            <p>Müşteri temsilcilerimiz en kısa sürede sizinle iletişime geçerek detaylı bilgi verecektir.</p>
                            <br>
                            <p>Saygılarımızla,<br>TEFEKKÜR TURİZM</p>");
                }

                TempData["Success"] = "Umre rezervasyon talebiniz alınmıştır. Teşekkür ederiz.";
                return RedirectToAction("UmreFormu");
            }
            return View(model);
        }
        [HttpGet]
        [Route("yorum-yap")]
        public IActionResult AddTestimonial()
        {
            return View();
        }

        [HttpPost]
        [Route("yorum-yap")]
        public async Task<IActionResult> AddTestimonial(Testimonial model)
        {
            ModelState.Remove("ImageUrl"); 

            if (ModelState.IsValid)
            {
                model.IsActive = false;
                model.Order = 99;

                if (model.ImageFile != null)
                {
                    string wwwRootPath = _hostEnvironment.WebRootPath;
                    string fileName = Path.GetFileNameWithoutExtension(model.ImageFile.FileName);
                    string extension = Path.GetExtension(model.ImageFile.FileName);
                    model.ImageUrl = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                    string path = Path.Combine(wwwRootPath + "/images/", model.ImageUrl);
                    using (var fileStream = new FileStream(path, FileMode.Create)) await model.ImageFile.CopyToAsync(fileStream);
                }

                _context.Testimonials.Add(model);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Yorumunuz alınmıştır. Yönetici onayından sonra yayınlanacaktır. Teşekkür ederiz.";
                return RedirectToAction("Index");
            }
            return View(model);
        }

        [Route("bloglar")]
        [Route("haberler")]
        public async Task<IActionResult> Blogs(int page = 1, string search = null)
{
    int pageSize = 6;
    var query = _context.BlogPosts.AsQueryable();

    if (!string.IsNullOrEmpty(search))
    {
        query = query.Where(b => b.Title.Contains(search) || b.Content.Contains(search));
        ViewBag.SearchTerm = search;
    }

    query = query.Where(b => b.IsActive).OrderByDescending(b => b.PublishedDate);
    
    var totalItems = await query.CountAsync();
    var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
    
    var blogs = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
    
    ViewBag.CurrentPage = page;
    ViewBag.TotalPages = totalPages;
    
    return View(blogs);
}        


        [HttpGet]
        [Route("sizi-arayalim")]
        [Route("Home/RequestCall")]
        public IActionResult RequestCall()
        {
            return View();
        }

        [HttpPost]
        [Route("sizi-arayalim")]
        [Route("Home/RequestCall")]
        public async Task<IActionResult> RequestCall(string fullName, string phone, string tourName)
        {
            if (string.IsNullOrEmpty(fullName) || string.IsNullOrEmpty(phone))
            {
                 if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                 {
                     return Json(new { success = false, message = "Lütfen ad soyad ve telefon bilgisi giriniz." });
                 }
                 
                 TempData["Error"] = "Lütfen ad soyad ve telefon bilgisi giriniz.";
                 return View();
            }

            var req = new CallRequest
            {
                FullName = fullName,
                Phone = phone,
                TourName = tourName,
                CreatedAt = DateTime.Now,
                IsProcessed = false
            };

            _context.CallRequests.Add(req);
            await _context.SaveChangesAsync();

            // Notifications
            await SendAdminNotification("Yeni Arama Talebi", $"<p><strong>İsim:</strong> {fullName}</p><p><strong>Telefon:</strong> {phone}</p><p><strong>Konu:</strong> {tourName}</p>");

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return Json(new { success = true, message = "Talebiniz alınmıştır. Müşteri temsilcilerimiz en kısa sürede sizi arayacaktır." });
            }

            TempData["Success"] = "Talebiniz alınmıştır. Müşteri temsilcilerimiz en kısa sürede sizi arayacaktır.";
            return RedirectToAction("RequestCall");

        }

        private async Task SendAdminNotification(string subject, string body) 
        {
            try {
                var settings = await _context.SiteSettings.ToListAsync();
                var host = settings.FirstOrDefault(s => s.Key == "Mail_Host")?.Value;
                var portStr = settings.FirstOrDefault(s => s.Key == "Mail_Port")?.Value;
                var user = settings.FirstOrDefault(s => s.Key == "Mail_User")?.Value;
                var pass = settings.FirstOrDefault(s => s.Key == "Mail_Password")?.Value;
                var contactEmail = settings.FirstOrDefault(s => s.Key == "Contact_Email")?.Value; 
                
                if(!string.IsNullOrEmpty(host) && !string.IsNullOrEmpty(user) && !string.IsNullOrEmpty(pass) && int.TryParse(portStr, out int port))
                {
                    var recipient = contactEmail ?? user; 
                    await _emailService.SendEmailAsync(host, port, user, pass, recipient, subject, body);
                }
            }
            catch {}
        }
        
        private async Task SendUserConfirmation(string email, string subject, string body)
        {
            try {
                var settings = await _context.SiteSettings.ToListAsync();
                var host = settings.FirstOrDefault(s => s.Key == "Mail_Host")?.Value;
                var portStr = settings.FirstOrDefault(s => s.Key == "Mail_Port")?.Value;
                var user = settings.FirstOrDefault(s => s.Key == "Mail_User")?.Value;
                var pass = settings.FirstOrDefault(s => s.Key == "Mail_Password")?.Value;
                
                if(!string.IsNullOrEmpty(host) && !string.IsNullOrEmpty(user) && !string.IsNullOrEmpty(pass) && int.TryParse(portStr, out int port) && !string.IsNullOrEmpty(email))
                {
                    await _emailService.SendEmailAsync(host, port, user, pass, email, subject, body);
                }
            }
            catch {}
        }

        [Route("arama")]
        public async Task<IActionResult> Search(string q)
        {
            if (string.IsNullOrWhiteSpace(q)) return RedirectToAction("Index");

            var packages = await _context.Packages
                .Where(p => p.IsActive && (p.Title.Contains(q) || p.Description.Contains(q)))
                .ToListAsync();

            var blogs = await _context.BlogPosts
                .Where(b => b.IsActive && (b.Title.Contains(q) || b.Content.Contains(q)))
                .ToListAsync();

            ViewBag.Query = q;
            ViewBag.Packages = packages;
            ViewBag.Blogs = blogs;

            return View();
        }

        [Route("degerlerimiz/{id}/{slug?}")]
        public async Task<IActionResult> FeatureDetails(int id, string slug = null)
        {
            var feature = await _context.Features.FirstOrDefaultAsync(f => f.Id == id && f.IsActive);
            if (feature == null) return NotFound();
            
            
            string subGroup = "";
            var title = feature.Title ?? "";

            if (title.Contains("Otel", StringComparison.OrdinalIgnoreCase) || title.Contains("Konaklama", StringComparison.OrdinalIgnoreCase)) 
                subGroup = "HotelSub";
            else if (title.Contains("Rehber", StringComparison.OrdinalIgnoreCase)) 
                subGroup = "GuideSub";
            else if (title.Contains("Yemek", StringComparison.OrdinalIgnoreCase) || title.Contains("Gıda", StringComparison.OrdinalIgnoreCase)) 
                subGroup = "FoodSub";

            if (!string.IsNullOrEmpty(subGroup))
            {
                ViewBag.RelatedMedia = await _context.Features
                    .Where(f => f.IsActive && f.Group == subGroup)
                    .OrderByDescending(f => f.Id)
                    .ToListAsync();
            }

            return View(feature);
        }

        [Route("umre-hazirlik-listesi")]
        [Route("valiz-hazirlama")]
        public IActionResult UmreHazirlik()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = System.Diagnostics.Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
