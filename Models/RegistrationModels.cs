using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Umre.Web.Models
{
    public class HacRegistration
    {
        public int Id { get; set; }

        [Display(Name = "Ad Soyad")]
        [Required(ErrorMessage = "Ad Soyad zorunludur.")]
        public string FullName { get; set; } = "";

        [Display(Name = "Telefon")]
        [Required(ErrorMessage = "Telefon zorunludur.")]
        public string Phone { get; set; } = "";

        [Display(Name = "E-Posta")]
        public string? Email { get; set; }

        [Display(Name = "Şehir")]
        public string? City { get; set; }

        [Display(Name = "Notlar")]
        public string? Note { get; set; }

        [Display(Name = "Kişi Sayısı")]
        public int PersonCount { get; set; } = 1;

        [Display(Name = "Oda Tipi")]
        public string? RoomType { get; set; }

        public bool IsRead { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }

    public class UmreReservation
    {
        public int Id { get; set; }

        [Display(Name = "Ad Soyad")]
        [Required(ErrorMessage = "Ad Soyad zorunludur.")]
        public string FullName { get; set; } = "";

        [Display(Name = "Telefon")]
        [Required(ErrorMessage = "Telefon zorunludur.")]
        public string Phone { get; set; } = "";

        [Display(Name = "E-Posta")]
        public string? Email { get; set; }

        [Display(Name = "Şehir")]
        public string? City { get; set; }

        [Display(Name = "Tercih Edilen Tarih")]
        public string? PreferredDate { get; set; }

        [Display(Name = "Kişi Sayısı")]
        public int PersonCount { get; set; } = 1;

        [Display(Name = "Oda Tipi")]
        public string? RoomType { get; set; }

        [Display(Name = "Notlar")]
        public string? Note { get; set; }

        public bool IsRead { get; set; } = false;

        // ✅ EKLENDİ: Admin panel için Durum alanı
        [Display(Name = "Durum")]
        public string Status { get; set; } = "Beklemede";

        // ✅ EKLENDİ: Soft delete için
        public bool IsDeleted { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public int? PackageId { get; set; }
        public virtual Package? Package { get; set; }
    }
}
