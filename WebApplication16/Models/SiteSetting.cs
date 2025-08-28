using System.ComponentModel.DataAnnotations;

namespace WebApplication16.Models
{
    public class SiteSetting
    {
        [Key] // کلید اصلی، نام منحصر به فرد تنظیمات خواهد بود (مثلاً "SiteName")
        [StringLength(100)]
        public string Key { get; set; }

        public string? Value { get; set; } // مقدار تنظیمات به صورت رشته ذخیره می‌شود
    }
}