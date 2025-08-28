using System.ComponentModel.DataAnnotations;

namespace WebApplication16.ViewModels
{
    public class SettingsViewModel
    {
        [Display(Name = "عنوان سایت")]
        public string? SiteName { get; set; }

        [Display(Name = "توضیحات سایت (برای SEO)")]
        [DataType(DataType.MultilineText)] // به ویو می‌گوید که از textarea استفاده کند
        public string? SiteDescription { get; set; }

        // پیامد: در آینده می‌توانیم به راحتی فیلدهای پیچیده‌تری اضافه کنیم،
        // بدون اینکه نگران نحوه ذخیره‌سازی آن‌ها باشیم.
        // [Display(Name = "لوگوی سایت")]
        // public IFormFile? SiteLogo { get; set; }
    }
}
