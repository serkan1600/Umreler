using System;
using System.ComponentModel.DataAnnotations;

namespace Umre.Web.Models
{
    public class Booking
    {
        [Key]
        public int Id { get; set; }

        public int PackageId { get; set; }
        public virtual Package? Package { get; set; }

        [Required(ErrorMessage = "Ad Soyad zorunludur.")]
        [Display(Name = "Ad Soyad")]
        public string CustomerName { get; set; } = "";

        [Required(ErrorMessage = "Telefon zorunludur.")]
        [Display(Name = "Telefon")]
        public string Phone { get; set; } = "";

        [EmailAddress]
        [Display(Name = "E-Posta")]
        public string? Email { get; set; }

        [Display(Name = "Notunuz")]
        public string? Note { get; set; }

        // ✅ Admin panel için durum
        [Display(Name = "Durum")]
        public string Status { get; set; } = "Beklemede";

        // ✅ Soft delete
        public bool IsDeleted { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
