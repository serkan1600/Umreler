using System.Collections.Generic;
using Umre.Web.Models;

namespace Umre.Web.ViewModels
{
    public class AdminSearchViewModel
    {
        public string Query { get; set; }
        public List<Package> Packages { get; set; } = new List<Package>();
        public List<Booking> Bookings { get; set; } = new List<Booking>();
        public List<BlogPost> BlogPosts { get; set; } = new List<BlogPost>();
        public List<ContactMessage> ContactMessages { get; set; } = new List<ContactMessage>();
        public List<Menu> Navigations { get; set; } = new List<Menu>();
    }
}
