using System.ComponentModel.DataAnnotations;
using WebApplication16.Enums;

namespace WebApplication16.Models
{
    public class FormField : BaseEntity
    {
        public int FormId { get; set; }
        public Form Form { get; set; }

        [Required(ErrorMessage = "نوع فیلد الزامی است.")]
        [Display(Name = "نوع فیلد")]
        public FieldType FieldType { get; set; }

        [Required(ErrorMessage = "برچسب فیلد الزامی است.")]
        [Display(Name = "برچسب")]
        public string Label { get; set; }

        [Display(Name = "نام برنامه‌نویسی")]
        public string? Name { get; set; }

        [Display(Name = "فیلد الزامی است؟")]
        public bool IsRequired { get; set; }

        [Display(Name = "ترتیب")]
        public int Order { get; set; }

        [Display(Name = "تنظیمات (JSON)")]
        public string? SettingsJson { get; set; } // برای لیست کشویی، دکمه‌های رادیویی، placeholder و ...

        [Display(Name = "قوانین اعتبارسنجی (JSON)")]
        public string? ValidationRulesJson { get; set; } // برای حداقل/حداکثر طول، نوع فایل و ...

        [Display(Name = "منطق شرطی (JSON)")]
        public string? ConditionalLogicJson { get; set; } // برای نمایش/پنهان شدن بر اساس فیلدهای دیگر

        [Display(Name = "کلاس‌های CSS")]
        public string? CssClasses { get; set; }
    }
}
