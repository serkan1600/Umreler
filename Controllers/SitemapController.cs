using System;
using System.Text;
using System.Xml.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Umre.Web.Data;

namespace Umre.Web.Controllers
{
    public class SitemapController : Controller
    {
        private readonly AppDbContext _context;

        public SitemapController(AppDbContext context)
        {
            _context = context;
        }

        [Route("sitemap.xml")]
        public async Task<IActionResult> Index()
        {
            var baseUrl = $"{Request.Scheme}://{Request.Host}";
            var sitemapNodes = new List<SitemapNode>();

            // Static Pages
            sitemapNodes.Add(new SitemapNode(baseUrl + "/", DateTime.Now, "daily", 1.0));
            sitemapNodes.Add(new SitemapNode(baseUrl + "/hakkimizda", DateTime.Now.AddDays(-1), "monthly", 0.8));
            sitemapNodes.Add(new SitemapNode(baseUrl + "/hac-turlari", DateTime.Now, "weekly", 0.9));
            sitemapNodes.Add(new SitemapNode(baseUrl + "/umre-turlari", DateTime.Now, "weekly", 0.9));
            sitemapNodes.Add(new SitemapNode(baseUrl + "/kultur-turlari", DateTime.Now, "weekly", 0.9));
            sitemapNodes.Add(new SitemapNode(baseUrl + "/kudus-turlari", DateTime.Now, "weekly", 0.8));
            sitemapNodes.Add(new SitemapNode(baseUrl + "/ozbekistan-turlari", DateTime.Now, "weekly", 0.8));
            sitemapNodes.Add(new SitemapNode(baseUrl + "/balkan-turlari", DateTime.Now, "weekly", 0.8));
            sitemapNodes.Add(new SitemapNode(baseUrl + "/galeri", DateTime.Now, "weekly", 0.7));
            sitemapNodes.Add(new SitemapNode(baseUrl + "/iletisim", DateTime.Now.AddMonths(-1), "yearly", 0.5));
            sitemapNodes.Add(new SitemapNode(baseUrl + "/haberler", DateTime.Now, "daily", 0.8));

            // Dynamic Packages
            var packages = await _context.Packages.Where(p => p.IsActive).ToListAsync();
            foreach (var pkg in packages)
            {
                sitemapNodes.Add(new SitemapNode(baseUrl + "/tur-detay/" + pkg.Id, DateTime.Now, "weekly", 0.8));
            }

            // Dynamic Blogs
            var blogs = await _context.BlogPosts.Where(b => b.IsActive).ToListAsync();
            foreach (var blog in blogs)
            {
                sitemapNodes.Add(new SitemapNode(baseUrl + "/haber/" + blog.Id, blog.PublishedDate, "monthly", 0.7));
            }
            
            // Dynamic Features/Values
            var features = await _context.Features.Where(f => f.IsActive).ToListAsync();
             foreach (var f in features)
            {
                 var slugI = f.Title.ToLower()
                                     .Replace("ş","s").Replace("ı","i").Replace("ğ","g").Replace("ü","u").Replace("ö","o").Replace("ç","c")
                                     .Replace(" ", "-").Replace("'", "").Replace("\"", "");
                sitemapNodes.Add(new SitemapNode(baseUrl + $"/degerlerimiz/{f.Id}/{slugI}", DateTime.Now, "monthly", 0.6));
            }
            
            // Services
            var services = await _context.Services.Where(s => s.IsActive).ToListAsync();
             foreach (var s in services)
            {
                sitemapNodes.Add(new SitemapNode(baseUrl + "/hizmetler/" + s.Slug, DateTime.Now, "monthly", 0.6));
            }


            // Generate XML
            XNamespace xmlns = "http://www.sitemaps.org/schemas/sitemap/0.9";
            var root = new XElement(xmlns + "urlset",
                from node in sitemapNodes
                select new XElement(xmlns + "url",
                    new XElement(xmlns + "loc", node.Url),
                    new XElement(xmlns + "lastmod", node.LastModified.ToString("yyyy-MM-dd")),
                    new XElement(xmlns + "changefreq", node.ChangeFrequency),
                    new XElement(xmlns + "priority", node.Priority.ToString("F1"))
                )
            );

            return Content(root.ToString(), "application/xml", Encoding.UTF8);
        }

        [Route("robots.txt")]
        public IActionResult Robots()
        {
            var baseUrl = $"{Request.Scheme}://{Request.Host}";
            var sb = new StringBuilder();
            sb.AppendLine("User-agent: *");
            sb.AppendLine("Disallow: /Admin/");
            sb.AppendLine("Disallow: /Home/Subscribe");
            sb.AppendLine("Disallow: /Home/RequestCall");
            sb.AppendLine($"Sitemap: {baseUrl}/sitemap.xml");

            return Content(sb.ToString(), "text/plain", Encoding.UTF8);
        }
    }

    public class SitemapNode
    {
        public string Url { get; set; }
        public DateTime LastModified { get; set; }
        public string ChangeFrequency { get; set; }
        public double Priority { get; set; }

        public SitemapNode(string url, DateTime lastModified, string changeFrequency, double priority)
        {
            Url = url;
            LastModified = lastModified;
            ChangeFrequency = changeFrequency;
            Priority = priority;
        }
    }
}
