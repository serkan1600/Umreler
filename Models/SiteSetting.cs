using System.ComponentModel.DataAnnotations;

namespace Umre.Web.Models
{
    public class SiteSetting
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Key { get; set; } 

        public string? Value { get; set; }
    }
}
