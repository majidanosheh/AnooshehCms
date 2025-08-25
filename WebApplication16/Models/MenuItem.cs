using System.ComponentModel.DataAnnotations;

namespace WebApplication16.Models
{
    public class MenuItem :BaseEntity
    {

        [Required]
        [StringLength(100)]
        [Display(Name = "عنوان لینک")]
        public string Title { get; set; } // مثال: "درباره ما"

        [Display(Name = "آدرس URL")]
        public string Url { get; set; }

        [Display(Name = "ترتیب نمایش")]
        public int Order { get; set; }

        // کلید خارجی برای منوی والد
        public int MenuId { get; set; }

        public Menu Menu { get; set; }

        [Display(Name = "باز شدن در تب جدید")]
        public bool OpenInNewTab { get; set; }

        // کلید خارجی برای آیتم والد (برای زیرمنوها)
        public int? ParentMenuItemId { get; set; }
        public MenuItem ParentMenuItem { get; set; }

        public List<MenuItem> SubMenuItems { get; set; } = new List<MenuItem>();
    }
}
