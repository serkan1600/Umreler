using System.ComponentModel.DataAnnotations;

namespace Umre.Web.Models
{
    public class Menu
    {
        [Key]
        public int Id { get; set; }
        
        [Display(Name = "Başlık")]
        public string? Title { get; set; }
        
        [Display(Name = "URL")]
        public string? Url { get; set; }
        
        [Display(Name = "İkon (FontAwesome)")]
        public string? Icon { get; set; }
        
        [Display(Name = "Üst Menü ID")]
        public int? ParentId { get; set; }
        
        [Display(Name = "Sıra")]
        public int Order { get; set; }
        
        [Display(Name = "Aktif mi?")]
        public bool IsActive { get; set; }
        
        [Display(Name = "Admin Menüsü mü?")]
        public bool IsAdmin { get; set; } = true;

        [Display(Name = "Gerekli Rol")]
        public string? Role { get; set; } 
    }
}
