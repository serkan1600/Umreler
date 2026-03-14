using System.ComponentModel.DataAnnotations;

namespace Umre.Web.Models
{
    public class Popup
    {
        public int Id { get; set; }

        [Display(Name = "Başlık")]
        public string? Title { get; set; }

        [Display(Name = "Görsel URL")]
        public string? ImageUrl { get; set; }

        [Display(Name = "Yönlendirilecek Link")]
        public string? LinkUrl { get; set; }

        [Display(Name = "Aktif Mi?")]
        public bool IsActive { get; set; }

        [Display(Name = "Sıra")]
        public int? Order { get; set; }

        [System.ComponentModel.DataAnnotations.Schema.NotMapped]
        [Display(Name = "Görsel Yükle")]
        public Microsoft.AspNetCore.Http.IFormFile? ImageFile { get; set; }
    }
}
