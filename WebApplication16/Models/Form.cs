using System.ComponentModel.DataAnnotations;

namespace WebApplication16.Models
{
    public class Form : BaseEntity
    {
        [Required]
        public string Name { get; set; }
        public string? Description { get; set; }
        public string Slug { get; set; }
        public bool IsActive { get; set; } = true;
        public string? SettingsJson { get; set; } // برای ذخیره تنظیمات فرم به صورت JSON

        public ICollection<FormField> Fields { get; set; } = new List<FormField>();
        public ICollection<FormSubmission> Submissions { get; set; } = new List<FormSubmission>();
    }
}