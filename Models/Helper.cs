using System.Text.RegularExpressions;
using System.Text;
using System.Globalization;

namespace Umre.Web.Models
{
    public static class Helper
    {
        public static string ToSlug(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return "";

            text = text.ToLower(new CultureInfo("tr-TR")); 
            text = text.Replace("ı", "i").Replace("ğ", "g").Replace("ü", "u").Replace("ş", "s").Replace("ö", "o").Replace("ç", "c");
            
            // Remove invalid characters
            text = Regex.Replace(text, @"[^a-z0-9\s-]", "");

            // Convert multiple spaces into one space   
            text = Regex.Replace(text, @"\s+", " ").Trim();

            // Replace spaces by underscores
            text = Regex.Replace(text, @"\s", "_");

            return text;
        }
    }
}
