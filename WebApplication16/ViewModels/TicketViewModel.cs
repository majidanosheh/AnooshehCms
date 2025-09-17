using WebApplication16.Enums;

namespace WebApplication16.ViewModels
{
    public class TicketViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public TicketStatus Status { get; set; }
        public TicketPriority Priority { get; set; }
        public string UserEmail { get; set; }
        public string? AssignedToUserEmail { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
