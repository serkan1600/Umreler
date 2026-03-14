using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Umre.Web.Models
{
    public class Package
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Başlık zorunludur.")]
        [Display(Name = "Tur Başlığı")]
        public string Title { get; set; }

        [Display(Name = "Açıklama")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Fiyat zorunludur.")]
        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Fiyat")]
        public decimal Price { get; set; }

        [Display(Name = "Resim")]
        public string? ImageUrl { get; set; }

        [NotMapped]
        [Display(Name = "Kapak Resmi Yükle")]
        public IFormFile? ImageFile { get; set; }

        [Display(Name = "Başlangıç Tarihi")]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [Display(Name = "Bitiş Tarihi")]
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }

        [Display(Name = "Aktif Mi?")]
        public bool IsActive { get; set; } = true;

        [Display(Name = "Para Birimi")]
        public string Currency { get; set; } = "USD"; 

        [Display(Name = "Kategori")]
        public int? CategoryId { get; set; }
        
        [ForeignKey("CategoryId")]
        public virtual TourCategory? Category { get; set; }

        public virtual List<TourFlow>? TourFlows { get; set; }
    }
}
