using System;
using System.ComponentModel.DataAnnotations;

namespace Umre.Web.Models
{
    public class CallRequest
    {
        public int Id { get; set; }

        // ✅ Soft delete
        public bool IsDeleted { get; set; } = false;

        [Required(ErrorMessage = "Ad Soyad zorunludur")]
        [Display(Name = "Ad Soyad")]
        public string FullName { get; set; } = "";

        [Required(ErrorMessage = "Telefon zorunludur")]
        [Display(Name = "Telefon Numarası")]
        public string Phone { get; set; } = "";

        [Display(Name = "İlgilendiği Tur")]
        public string? TourName { get; set; }

        [Display(Name = "Talep Tarihi")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [Display(Name = "İşlendi Mi?")]
        public bool IsProcessed { get; set; } = false;

        // ✅ EKLENDİ (HATAYI BİTİRİR)
        [Display(Name = "Durum")]
        public string Status { get; set; } = "Beklemede";

        [Display(Name = "Admin Notu")]
        public string? AdminNote { get; set; }
    }
}
