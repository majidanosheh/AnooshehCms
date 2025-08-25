namespace WebApplication16.ViewModels
{
    public class AuditLogViewModel
    {
        public string UserEmail { get; set; }
        public string ActionType { get; set; }
        public string EntityName { get; set; }
        public string Details { get; set; }
        public DateTime Timestamp { get; set; }
    }
}