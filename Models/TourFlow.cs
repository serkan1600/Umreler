using System;
using System.ComponentModel.DataAnnotations;

namespace Umre.Web.Models
{
    public class TourFlow
    {
        public int Id { get; set; }

        public int PackageId { get; set; }
        public virtual Package Package { get; set; }

        [Required]
        [Display(Name = "Kaçıncı Gün")]
        public int DayNumber { get; set; }

        [Required]
        [Display(Name = "Başlık (Örn: İstanbul - Cidde)")]
        public string Title { get; set; }

        [Display(Name = "Açıklama")]
        public string Description { get; set; }

        public bool IsActive { get; set; } = true;
    }
}
