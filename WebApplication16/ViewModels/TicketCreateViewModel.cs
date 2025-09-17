using System.ComponentModel.DataAnnotations;
using WebApplication16.Enums;


namespace WebApplication16.ViewModels
{
    public class TicketCreateViewModel
    {
        [Required(ErrorMessage = "وارد کردن عنوان الزامی است")]
        [MaxLength(200)]
        [Display(Name = "عنوان تیکت")]
        public string Title { get; set; }

        [Required(ErrorMessage = "وارد کردن توضیحات الزامی است")]
        [Display(Name = "توضیحات")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Display(Name = "اولویت")]
        public TicketPriority Priority { get; set; } = TicketPriority.Medium;
    }
}
