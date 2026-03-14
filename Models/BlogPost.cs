using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Umre.Web.Models
{
    public class BlogPost
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Başlık")]
        public string? Title { get; set; }

        [Display(Name = "Özet")]
        public string? Summary { get; set; }

        [Display(Name = "İçerik")]
        public string? Content { get; set; }

        [Display(Name = "Kapak Resmi")]
        public string? ImageUrl { get; set; }

        [NotMapped]
        [Display(Name = "Resim Yükle")]
        public IFormFile? ImageFile { get; set; }

        [Display(Name = "Yayın Tarihi")]
        public DateTime PublishedDate { get; set; } = DateTime.Now;

        [Display(Name = "Aktif mi?")]
        public bool IsActive { get; set; } = true;
    }
}
