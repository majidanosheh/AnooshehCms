using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebApplication16.Enums;

namespace WebApplication16.Models
{
    public class AuditLog
    {
        public int Id { get; set; }
        public string? UserId { get; set; }
        public AuditType ActionType { get; set; } // استفاده از Enum برای خوانایی بیشتر
        public string EntityName { get; set; }
        public DateTime Timestamp { get; set; }
        public string PrimaryKey { get; set; } // کلید اصلی رکورد تغییر یافته

        public string? Details { get; set; }

        // برای ذخیره مقادیر به صورت JSON
        public string? OldValues { get; set; }
        public string? NewValues { get; set; }

        // برای ذخیره نام ستون‌هایی که تغییر کرده‌اند
        [NotMapped] // این فیلد در دیتابیس ذخیره نمی‌شود
        public List<string> ChangedColumns { get; set; } = new List<string>();
    }
}
