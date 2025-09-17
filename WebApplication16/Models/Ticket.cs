using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebApplication16.Enums;
using WebApplication16.Models;

namespace WebApplication16.Models
{
    public class Ticket : BaseEntity
    {
        [Required]
        [MaxLength(200)]
        [Display(Name = "عنوان تیکت")]
        public string Title { get; set; }

        [Required]
        [Display(Name = "توضیحات")]
        public string Description { get; set; }

        [Display(Name = "وضعیت")]
        public TicketStatus Status { get; set; } = TicketStatus.Open;

        [Display(Name = "اولویت")]
        public TicketPriority Priority { get; set; } = TicketPriority.Medium;

        [Display(Name = "کاربر ایجاد کننده")]
        public string UserId { get; set; } // شناسه کاربری که تیکت را ساخته
        [ForeignKey("UserId")]
        public IdentityUser User { get; set; }

        [Display(Name = "کاربر تخصیص یافته")]
        public string? AssignedToUserId { get; set; } // شناسه کاربری که تیکت به او تخصیص داده شده
        [ForeignKey("AssignedToUserId")]
        public IdentityUser? AssignedToUser { get; set; }

        public List<TicketReply> Replies { get; set; } = new List<TicketReply>();
    }
}