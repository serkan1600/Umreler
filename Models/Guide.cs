using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace Umre.Web.Models
{
    public class Guide
    {
        public int Id { get; set; }

        [Display(Name = "Ad Soyad")]
        public string? Name { get; set; }

        [Display(Name = "Unvan (Örn: Emekli Müftü)")]
        public string? Title { get; set; }

        [Display(Name = "Biyografi")]
        public string? Biography { get; set; }

        [Display(Name = "Resim URL")]
        public string? ImageUrl { get; set; }

        [NotMapped]
        [Display(Name = "Resim Yükle")]
        public IFormFile? ImageFile { get; set; }
        
        [Display(Name = "Sıra No")]
        public int Order { get; set; }

        public bool IsActive { get; set; } = true;

        [Display(Name = "Facebook URL")]
        public string? FacebookUrl { get; set; }

        [Display(Name = "Instagram URL")]
        public string? InstagramUrl { get; set; }
    }
}
