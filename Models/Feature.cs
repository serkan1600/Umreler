using System.ComponentModel.DataAnnotations;

namespace Umre.Web.Models
{
    public class Feature
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        
        [Display(Name = "Detaylı İçerik")]
        public string? Content { get; set; } 

        [Display(Name = "Video Bağlantısı (Youtube Embed)")]
        public string? VideoUrl { get; set; }

        public string? Icon { get; set; } 
        public int Order { get; set; }
        public bool IsActive { get; set; }

        [System.ComponentModel.DataAnnotations.Schema.NotMapped]
        [Display(Name = "Görsel (İkon Yerine)")]
        public Microsoft.AspNetCore.Http.IFormFile? ImageFile { get; set; }
        
        [Display(Name = "Resim URL")]
        public string? ImageUrl { get; set; }

        
        [Display(Name = "Gösterim Yeri")]
        public string Group { get; set; } = "HomePage";
    }
}
