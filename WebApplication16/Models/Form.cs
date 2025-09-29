using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace WebApplication16.Models
{
    public class Form : BaseEntity
    {
        [Required(ErrorMessage = "نام فرم الزامی است.")]
        [Display(Name = "نام فرم")]
        public string Name { get; set; }

        [Display(Name = "توضیحات")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "اسلاگ فرم (آدرس URL) الزامی است.")]
        [RegularExpression(@"^[a-z0-9\-]+$", ErrorMessage = "اسلاگ فقط می‌تواند شامل حروف کوچک انگلیسی، اعداد و خط تیره باشد.")]
        [Display(Name = "اسلاگ")]
        public string Slug { get; set; }

        [Display(Name = "فعال است؟")]
        public bool IsActive { get; set; } = true;

        [Display(Name = "تنظیمات (JSON)")]
        public string? SettingsJson { get; set; }

        [Display(Name = "کلاس‌های CSS")]
        public string? CssClasses { get; set; }

        public ICollection<FormField> FormFields { get; set; } = new List<FormField>();
        public ICollection<FormSubmission> Submissions { get; set; } = new List<FormSubmission>();
    }
}
