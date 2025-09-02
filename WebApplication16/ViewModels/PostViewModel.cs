using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace WebApplication16.ViewModels
{
    public class PostViewModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Title { get; set; }

        [Required]
        [StringLength(200)]
        public string Slug { get; set; }

        public string Content { get; set; }

        [StringLength(500)]
        public string? Excerpt { get; set; }

        public IFormFile? FeaturedImage { get; set; } // برای آپلود تصویر جدید
        public string? FeaturedImageUrl { get; set; } // برای نمایش تصویر فعلی

        public bool IsPublished { get; set; }

        [Required]
        public int CategoryId { get; set; }

        // برای انتخاب چندین تگ
        public List<int> SelectedTagIds { get; set; } = new List<int>();

        // --- پراپرتی‌های کمکی برای پر کردن دراپ‌دان‌ها در ویو ---
        public SelectList? Categories { get; set; }
        public MultiSelectList? Tags { get; set; }

        [MaxLength(160, ErrorMessage = "حداکثر طول عنوان متا ۱۶۰ کاراکتر است")]
        [Display(Name = "عنوان متا (SEO)")]
        public string? MetaTitle { get; set; }

        [MaxLength(300, ErrorMessage = "حداکثر طول توضیحات متا ۳۰۰ کاراکتر است")]
        [Display(Name = "توضیحات متا (SEO)")]
        [DataType(DataType.MultilineText)]
        public string? MetaDescription { get; set; }
    }
}