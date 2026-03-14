using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Umre.Web.Data;
using Umre.Web.Models;

namespace Umre.Web.Controllers
{
    public class BlogController : Controller
    {
        private readonly AppDbContext _context;

        public BlogController(AppDbContext context)
        {
            _context = context;
        }

        
        public async Task<IActionResult> Index(string search, int page = 1)
        {
            var pageSize = 6;
            var query = _context.BlogPosts.Where(b => b.IsActive).AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(b => b.Title.Contains(search) || b.Content.Contains(search));
                ViewBag.SearchTerm = search;
                ViewBag.HeaderTitle = $"Arama Sonuçları: {search}";
                ViewBag.HeaderDesc = $"'{search}' için bulunan blog yazıları";
            }
            else
            {
                ViewBag.HeaderTitle = "Blog & Haberler";
                ViewBag.HeaderDesc = "Hac, Umre ve Kudüs turları hakkında güncel bilgiler, rehberler ve haberler.";
            }

            var totalItems = await query.CountAsync();
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
            
            var items = await query
                .OrderByDescending(b => b.PublishedDate)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;

            
            ViewBag.RecentPosts = await _context.BlogPosts
                .Where(b => b.IsActive)
                .OrderByDescending(b => b.PublishedDate)
                .Take(5)
                .ToListAsync();

            return View(items);
        }

        
        public async Task<IActionResult> Details(int id)
        {
            var post = await _context.BlogPosts.FirstOrDefaultAsync(b => b.Id == id && b.IsActive);
            
            if (post == null)
            {
                return NotFound();
            }

            
            ViewBag.RecentPosts = await _context.BlogPosts
                .Where(b => b.IsActive && b.Id != id) 
                .OrderByDescending(b => b.PublishedDate)
                .Take(5)
                .ToListAsync();

            return View(post);
        }
    }
}
