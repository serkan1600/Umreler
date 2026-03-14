using System.ComponentModel.DataAnnotations;

namespace Umre.Web.Models
{
    public class UmreForm
    {
        public int Id { get; set; }

        [Display(Name = "Ad Soyad")]
        public string? FullName { get; set; }

        [Display(Name = "Genel Memnuniyet")]
        public int Satisfaction { get; set; } 

        [Display(Name = "Rehberlik")]
        public int GuideRating { get; set; } 

        [Display(Name = "Otel")]
        public int HotelRating { get; set; } 
        
        [Display(Name = "Ulaşım")]
        public int TransportationRating { get; set; } 
        
        [Display(Name = "Yemek")]
        public int FoodRating { get; set; } 

        [Display(Name = "Görüş ve Öneriler")]
        public string? Comments { get; set; }

        public bool IsRead { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
