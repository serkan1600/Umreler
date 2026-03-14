using System;

namespace Umre.Web.Models
{
    public class Countdown
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Subtitle { get; set; }
        public DateTime TargetDate { get; set; }
        public string? ButtonText { get; set; }
        public string? ButtonUrl { get; set; }
        public bool IsActive { get; set; }
    }
}
