using System.ComponentModel.DataAnnotations;

namespace Umre.Web.Models
{
    public class UmreGuide
    {
        public int Id { get; set; }
        [Display(Name = "Başlık")]
        public string Title { get; set; }
        [Display(Name = "İçerik")]
        public string Content { get; set; }
        public bool IsActive { get; set; } = true;
    }

    public class UmreHotel
    {
        public int Id { get; set; }
        [Display(Name = "Otel Adı")]
        public string? Name { get; set; }
        [Display(Name = "Açıklama")]
        public string? Description { get; set; }
        [Display(Name = "Şehir")]
        public string? City { get; set; } 
        [Display(Name = "Resim URL")]
        public string? ImageUrl { get; set; }
        
        [System.ComponentModel.DataAnnotations.Schema.NotMapped]
        [Display(Name = "Resim Yükle")]
        public Microsoft.AspNetCore.Http.IFormFile? ImageFile { get; set; }

        public bool IsActive { get; set; } = true;
    }

    public class UmreEducation
    {
        public int Id { get; set; }
        [Display(Name = "Eğitim Konusu")]
        public string Topic { get; set; }
        [Display(Name = "Açıklama")]
        public string Description { get; set; }
        [Display(Name = "Tarih/Saat")]
        public DateTime? EventDate { get; set; }
        [Display(Name = "Yer")]
        public string Location { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
