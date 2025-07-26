using System.ComponentModel.DataAnnotations;

namespace YourCmsName.Models // نام پروژه خود را جایگزین کنید
{
    public class Page :BaseEntity
    {

        [Required(ErrorMessage = "وارد کردن عنوان الزامی است")]
        [MaxLength(200)]
        public string Title { get; set; }

        public string Content { get; set; }

        [Required(ErrorMessage = "وارد کردن اسلاگ الزامی است")]
        public string Slug { get; set; } // برای آدرس URL

    }
}
