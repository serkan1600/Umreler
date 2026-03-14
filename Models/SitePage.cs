using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Umre.Web.Models
{
    // The New, Simplified Content Model
    // Keyed by SLUG, not by arbitrary IDs.
    public class SitePage
    {
        [Key]
        [MaxLength(200)]
        public string Slug { get; set; } // e.g. "kudus-turlari"

        [Display(Name = "Sayfa Başlığı")]
        public string Title { get; set; }

        [Display(Name = "İçerik")]
        public string? Content { get; set; }

        public string? ImageUrl { get; set; }

        [NotMapped]
        public IFormFile? ImageFile { get; set; }
        
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
    }
}
