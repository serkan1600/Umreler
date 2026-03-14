using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Umre.Web.Models
{
    public class Testimonial
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Müşteri Adı")]
        [Required(ErrorMessage = "İsim gereklidir.")]
        public string CustomerName { get; set; }

        [Display(Name = "Ünvan / Meslek")]
        public string? Title { get; set; } 

        [Display(Name = "Yorum")]
        [Required(ErrorMessage = "Yorum gereklidir.")]
        public string Comment { get; set; }

        [Display(Name = "Resim")]
        public string? ImageUrl { get; set; }

        [NotMapped]
        [Display(Name = "Resim Yükle")]
        public IFormFile? ImageFile { get; set; }

        [Display(Name = "Sıra")]
        public int Order { get; set; }

        [Display(Name = "Aktif mi?")]
        public bool IsActive { get; set; } = true;

        [Display(Name = "Oluşturulma Tarihi")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
