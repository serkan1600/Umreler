using System.ComponentModel.DataAnnotations;

namespace Umre.Web.Models
{
    public class Faq
    {
        public int Id { get; set; }

        [Display(Name = "Soru")]
        public string? Question { get; set; }

        [Display(Name = "Cevap")]
        public string? Answer { get; set; }

        [Display(Name = "Sıra")]
        public int? Order { get; set; }

        [Display(Name = "Aktif Mi?")]
        public bool IsActive { get; set; } = true;
    }
}
