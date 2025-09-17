using System.ComponentModel.DataAnnotations;
using WebApplication16.Models;
using WebApplication16.Enums;

namespace WebApplication16.Models
{
    public class FormField : BaseEntity
    {
        public int FormId { get; set; }
        public Form Form { get; set; }

        [Required]
        public FieldType FieldType { get; set; }
        [Required]
        public string Label { get; set; }
        public string? Name { get; set; } // نام برنامه‌نویسی
        public bool IsRequired { get; set; }
        public int Order { get; set; }
        public string? SettingsJson { get; set; } // برای تنظیمات خاص فیلد
        public string? ConditionalLogicJson { get; set; } // برای قوانین شرطی
    }
}
