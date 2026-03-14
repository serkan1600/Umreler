using System;
using System.ComponentModel.DataAnnotations;

namespace Umre.Web.Models
{
    public class NewEntity
    {
        [Key]
        public int Id { get; set; }
        public string? Name { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
