using System.ComponentModel.DataAnnotations;

namespace YourCmsName.Models // نام پروژه خود را جایگزین کنید
{
    public abstract class BaseEntity
    {
        public int Id { get; set; }

        [Display(Name = "تاریخ ایجاد")]
        public DateTime CreatedAt { get; set; }

        [Display(Name = "تاریخ آخرین ویرایش")]
        public DateTime? ModifiedAt { get; set; }

        // در اینجا ما شناسه کاربر را ذخیره می‌کنیم
        // که از نوع رشته (string) است، چون کلید اصلی در Identity به صورت پیش‌فرض string است.
        [Display(Name = "ایجاد شده توسط")]
        public string? CreatedBy { get; set; }

        [Display(Name = "ویرایش شده توسط")]
        public string? ModifiedBy { get; set; }
    }
}