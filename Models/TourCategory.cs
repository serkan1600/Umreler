using System.ComponentModel.DataAnnotations;

namespace Umre.Web.Models
{
    public class TourCategory
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Kategori Adı")]
        public string Name { get; set; }

        [Display(Name = "Url (Slug)")]
        public string Slug { get; set; } 

        [Display(Name = "Açıklama")]
        public string? Description { get; set; }

        [Display(Name = "Kapak Görseli")]
        public string? ImageUrl { get; set; }

        [System.ComponentModel.DataAnnotations.Schema.NotMapped]
        public Microsoft.AspNetCore.Http.IFormFile? ImageFile { get; set; }

        public bool IsActive { get; set; } = true;
    }
}
