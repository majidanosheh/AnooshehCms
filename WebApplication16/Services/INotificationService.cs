namespace WebApplication16.Services
{
    public interface INotificationService
    {
        Task SendEmailAsync(string toEmail, string subject, string message);
    }
}