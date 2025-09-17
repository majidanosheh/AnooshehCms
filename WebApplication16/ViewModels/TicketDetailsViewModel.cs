// File: ViewModels/TicketReplyViewModel.cs
using WebApplication16.Models;

namespace WebApplication16.ViewModels
{
    public class TicketReplyViewModel
    {
        public string Message { get; set; }
        public string UserEmail { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}

// File: ViewModels/TicketDetailsViewModel.cs
namespace WebApplication16.ViewModels
{
    public class TicketDetailsViewModel
    {
        public Ticket Ticket { get; set; }
        public List<TicketReplyViewModel> Replies { get; set; } = new List<TicketReplyViewModel>();

        // برای فرم پاسخ جدید
        public int TicketId { get; set; }
        public string NewReplyMessage { get; set; }
    }
}