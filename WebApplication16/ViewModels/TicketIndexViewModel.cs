using WebApplication16.ViewModels;

namespace YourCmsName.ViewModels
{
    public class TicketIndexViewModel
    {
        public int TotalTickets { get; set; }
        public int OpenTickets { get; set; }
        public int ClosedTickets { get; set; }
        public int RespondedTickets { get; set; }
        public PaginatedList<TicketViewModel> PaginatedTickets { get; set; }
    }
}