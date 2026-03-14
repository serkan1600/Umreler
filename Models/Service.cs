using System.ComponentModel.DataAnnotations;

namespace Umre.Web.Models
{
    public class Service
    {
        public int Id { get; set; }

        [Display(Name = "Hizmet Başlığı")]
        public string? Title { get; set; }

        [Display(Name = "URL (Slug)")]
        public string? Slug { get; set; } 

        [Display(Name = "İçerik")]
        public string? Content { get; set; }

        [Display(Name = "Kapak Resmi")]
        public string? ImageUrl { get; set; }

        [System.ComponentModel.DataAnnotations.Schema.NotMapped]
        [Display(Name = "Resim Yükle")]
        public Microsoft.AspNetCore.Http.IFormFile? ImageFile { get; set; }
        
        [Display(Name = "Sıra")]
        public int? Order { get; set; }

        [Display(Name = "Aktif Mi?")]
        public bool IsActive { get; set; } = true;
    }
}
