using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using WebApplication16.Enums;

namespace WebApplication16.ViewModels
{
    public class TicketAdminViewModel
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "عنوان")]
        public string Title { get; set; }

        [Required]
        [Display(Name = "توضیحات")]
        public string Description { get; set; }

        [Display(Name = "وضعیت")]
        public TicketStatus Status { get; set; }

        [Display(Name = "اولویت")]
        public TicketPriority Priority { get; set; }

        [Required(ErrorMessage = "انتخاب کاربر الزامی است")]
        [Display(Name = "کاربر ایجاد کننده")]
        public string UserId { get; set; }

        [Display(Name = "تخصیص یافته به")]
        public string? AssignedToUserId { get; set; }

        // پراپرتی‌های زیر برای پر کردن دراپ‌دان‌ها در ویو استفاده می‌شوند
        public SelectList? Users { get; set; }
        public SelectList? Statuses { get; set; }
        public SelectList? Priorities { get; set; }
    }
}