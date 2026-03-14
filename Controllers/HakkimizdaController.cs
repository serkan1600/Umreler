using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Umre.Web.Controllers
{
    public class HakkimizdaController : Controller
    {
        private readonly Umre.Web.Data.AppDbContext _context;

        public HakkimizdaController(Umre.Web.Data.AppDbContext context)
        {
            _context = context;
        }

        [Route("hakkimizda")]
        public async Task<IActionResult> Index()
        {
            var settings = await _context.SiteSettings.ToListAsync();
            ViewBag.Title = settings.FirstOrDefault(s => s.Key == "About_Title")?.Value ?? "Hakkımızda";
            ViewBag.Description = settings.FirstOrDefault(s => s.Key == "About_Description")?.Value ?? "İçerik hazırlanıyor...";
            
             // Optional: Fetch Vision/Mission if they exist in settings
            ViewBag.Vision = settings.FirstOrDefault(s => s.Key == "About_Vision")?.Value;
            ViewBag.Mission = settings.FirstOrDefault(s => s.Key == "About_Mission")?.Value;
            ViewBag.Values = settings.FirstOrDefault(s => s.Key == "About_Values")?.Value;
            
            return View();
        }
    }
}
