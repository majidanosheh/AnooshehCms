using System.ComponentModel.DataAnnotations;

namespace WebApplication16.ViewModels
{
    public class ContactFormViewModel
    {
        [Required(ErrorMessage = "وارد کردن نام الزامی است")]
        [Display(Name = "نام شما")]
        public string Name { get; set; }

        [Required(ErrorMessage = "وارد کردن ایمیل الزامی است")]
        [EmailAddress(ErrorMessage = "لطفاً یک آدرس ایمیل معتبر وارد کنید")]
        [Display(Name = "ایمیل شما")]
        public string Email { get; set; }

        [Required(ErrorMessage = "وارد کردن موضوع الزامی است")]
        [Display(Name = "موضوع")]
        public string Subject { get; set; }

        [Required(ErrorMessage = "وارد کردن متن پیام الزامی است")]
        [Display(Name = "پیام شما")]
        [DataType(DataType.MultilineText)]
        public string Message { get; set; }
    }
}