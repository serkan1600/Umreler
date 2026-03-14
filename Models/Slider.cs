using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Umre.Web.Models
{
    public class Slider
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Başlık")]
        public string? Title { get; set; }

        [Display(Name = "Alt Başlık")]
        public string? SubTitle { get; set; }

        [Display(Name = "Resim")]
        public string? ImageUrl { get; set; }

        [NotMapped]
        [Display(Name = "Resim Yükle")]
        public IFormFile? ImageFile { get; set; }

        [Display(Name = "Sıra")]
        public int SortOrder { get; set; } = 0;

        [Display(Name = "Aktif mi?")]
        public bool IsActive { get; set; } = true;

        [Display(Name = "Oluşturulma Tarihi")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
