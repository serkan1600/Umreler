using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Umre.Web.Models
{
    public class GalleryItem
    {
        [Key]
        public int Id { get; set; }
        
        [Display(Name = "Başlık")]
        public string? Title { get; set; }
        
        [Display(Name = "Açıklama")]
        public string? Description { get; set; }
        
        [Display(Name = "Dosya Yolu")]
        public string? FilePath { get; set; } 
        
        [NotMapped]
        [Display(Name = "Dosya Yükle")]
        public IFormFile? File { get; set; }

        [Display(Name = "Tür")]
        public string Type { get; set; } = "Image"; 

        [Display(Name = "Aktif")]
        public bool IsActive { get; set; } = true;

        [Display(Name = "Video Embed/Link")]
        public string? VideoEmbedUrl { get; set; }

        [Display(Name = "Sıra")]
        public int SortOrder { get; set; }

        [Display(Name = "Oluşturulma Tarihi")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
