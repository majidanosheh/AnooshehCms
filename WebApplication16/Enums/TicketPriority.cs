using System.ComponentModel.DataAnnotations;

namespace WebApplication16.Enums
{
    public enum TicketPriority
    {
        [Display(Name = "پایین")]
        Low,

        [Display(Name = "متوسط")]
        Medium,

        [Display(Name = "بالا")]
        High,

        [Display(Name = "فوری")]
        Urgent
    }
}
