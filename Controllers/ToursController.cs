using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Umre.Web.Data;
using Umre.Web.Models;

namespace Umre.Web.Controllers
{
    public class ToursController : Controller
    {
        private readonly AppDbContext _context;

        public ToursController(AppDbContext context)
        {
            _context = context;
        }

        [Route("kudus-turlari")]
        public async Task<IActionResult> Kudus() => await LoadPage("kudus-turlari");

        [Route("ozbekistan-turlari")]
        public async Task<IActionResult> Ozbekistan() => await LoadPage("ozbekistan-turlari");

        [Route("balkan-turlari")]
        public async Task<IActionResult> Balkan() => await LoadPage("balkan-turlari");

        private async Task<IActionResult> LoadPage(string slug)
        {
            var page = await _context.SitePages.FirstOrDefaultAsync(x => x.Slug == slug);
            
            if (page == null)
            {
                // If page doesn't exist in DB yet, show empty/default
                // But do NOT show "Not Found" to avoid panic. Show generic template waiting for admin input.
                 string defaultImg = "/images/slider1.jpg";
                 if (slug.Contains("kudus")) defaultImg = "/images/kudus_cat.jpg";
                 else if (slug.Contains("ozbekistan")) defaultImg = "/images/ozbek_cat.jpg";
                 else if (slug.Contains("balkan")) defaultImg = "/images/balkan_cat.jpg";

                 page = new SitePage 
                 { 
                     Slug = slug, 
                     Title = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(slug.Replace("-", " ")),
                     Content = "",
                     ImageUrl = defaultImg
                 };
            }

            // Also fetch packages related to this tour (Simple keyword match for fallback)
            // e.g. "kudus"
            var keyword = slug.Replace("-turlari", "").Replace("turlari", "");
            var packages = await _context.Packages
                .Include(p => p.Category)
                .Where(p => p.IsActive && (p.Category.Slug.Contains(keyword) || p.Title.ToLower().Contains(keyword)))
                .OrderBy(p => p.StartDate)
                .ToListAsync();

            ViewBag.Packages = packages;

            return View("Detail", page);
        }
    }
}
