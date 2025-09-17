using System.ComponentModel.DataAnnotations;

namespace WebApplication16.ViewModels
{
    public class MenuItemViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "وارد کردن عنوان الزامی است")]
        [Display(Name = "عنوان لینک")]
        public string Title { get; set; }

        [Display(Name = "آدرس URL")]
        public string Url { get; set; }

        [Display(Name = "ترتیب نمایش")]
        public int Order { get; set; }
        [Display(Name = "باز شدن در تب جدید")]
        public bool OpenInNewTab { get; set; }
        public int MenuId { get; set; } // برای بازگشت به صفحه صحیح
        public string? IconClass { get; set; }
        [Display(Name = "آیتم والد (برای ساخت زیرمنو)")]
        public int? ParentMenuItemId { get; set; }

        [Display(Name = "مجوز مورد نیاز")]
        public string? RequiredPermission { get; set; }
        public List<MenuItemViewModel> SubItems { get; set; } = new List<MenuItemViewModel>();

    }
}