using System.ComponentModel.DataAnnotations;

namespace WebApplication16.Enums
{
    public enum TicketStatus
    {
        [Display(Name = "باز")]
        Open,

        [Display(Name = "در حال بررسی")]
        InProgress,

        [Display(Name = "پاسخ داده شده")]
        Answered,

        [Display(Name = "بسته شده")]
        Closed
    }
}