using System.ComponentModel.DataAnnotations;
namespace WebApplication16.Enums
{
    public enum SubmissionStatus
    {
        [Display(Name = "جدید")] New,
        [Display(Name = "خوانده شده")] Read,
        [Display(Name = "پاسخ داده شده")] Replied
    }
}
