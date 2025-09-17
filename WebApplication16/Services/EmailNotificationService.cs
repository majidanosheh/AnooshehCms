using MailKit.Net.Smtp;
using MimeKit;
using WebApplication16.Services;

namespace WebApplication16.Services
{
    public class EmailNotificationService : INotificationService
    {
        private readonly IConfiguration _configuration;

        public EmailNotificationService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string message)
        {
            // خواندن تنظیمات SMTP از appsettings.json
            var smtpSettings = _configuration.GetSection("SmtpSettings");
            var fromAddress = smtpSettings["FromAddress"];
            var smtpServer = smtpSettings["Server"];
            var smtpPort = int.Parse(smtpSettings["Port"]);
            var smtpUsername = smtpSettings["Username"];
            var smtpPassword = smtpSettings["Password"];

            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress("", fromAddress));
            emailMessage.To.Add(new MailboxAddress("", toEmail));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart("html") { Text = message };

            using (var client = new SmtpClient())
            {
                // برای تست می‌توانید از سرویس‌هایی مانند Mailtrap.io استفاده کنید
                await client.ConnectAsync(smtpServer, smtpPort, MailKit.Security.SecureSocketOptions.StartTls);
                await client.AuthenticateAsync(smtpUsername, smtpPassword);
                await client.SendAsync(emailMessage);
                await client.DisconnectAsync(true);
            }
        }
    }
}