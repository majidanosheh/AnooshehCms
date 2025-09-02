using System.ComponentModel.DataAnnotations;

namespace WebApplication16.Models // نام پروژه خود را جایگزین کنید
{
    public class Page : BaseEntity
    {

        [Required(ErrorMessage = "وارد کردن عنوان الزامی است")]
        [MaxLength(200)]
        public string Title { get; set; }

        public string Content { get; set; }

        [Required(ErrorMessage = "وارد کردن اسلاگ الزامی است")]
        public string Slug { get; set; } // برای آدرس URL

        [MaxLength(160, ErrorMessage = "حداکثر طول عنوان متا ۱۶۰ کاراکتر است")]
        [Display(Name = "عنوان متا (SEO)")]
        public string? MetaTitle { get; set; }

        [MaxLength(300, ErrorMessage = "حداکثر طول توضیحات متا ۳۰۰ کاراکتر است")]
        [Display(Name = "توضیحات متا (SEO)")]
        [DataType(DataType.MultilineText)]
        public string? MetaDescription { get; set; }

    }
}
