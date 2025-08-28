namespace WebApplication16.ViewModels
{
    /// <summary>
    /// این کلاس به عنوان یک DTO (Data Transfer Object) عمل می‌کند
    /// و وظیفه آن، نگهداری تنظیمات خوانده شده از پایگاه داده به صورت یک شیء ساختاریافته است.
    /// </summary>
    public class SiteSettings
    {
        public string? SiteName { get; set; }
        public string? SiteDescription { get; set; }
    }
}