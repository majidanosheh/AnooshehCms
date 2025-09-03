using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace WebApplication16.Models
{
    public class AllowedFileType :BaseEntity
    {
        [Required]
        [Display(Name = "پسوند فایل (با نقطه)")]
        public string Extension { get; set; } // مثال: ".pdf"

        [Required]
        [Display(Name = "امضای فایل (Hex)")]
        [Comment("امضاهای مختلف را با کاما جدا کنید. مثال: FFD8FFE0,FFD8FFE1")]
        public string Signatures { get; set; } // مثال: "25504446"

        [Required]
        [Display(Name = "نوع MIME")]
        public string MimeType { get; set; } // مثال: "application/pdf"

        [Display(Name = "حداکثر حجم (بایت)")]
        public long MaxSizeInBytes { get; set; } = 5 * 1024 * 1024; // پیش‌فرض: ۵ مگابایت

        [Display(Name = "توضیحات")]
        public string? Description { get; set; }
    }
}
