using MailKit.Net.Smtp;
using MimeKit;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace CuaHangVHT.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _config;

        public EmailService(IConfiguration config)
        {
            _config = config;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string message)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress(_config["EmailSettings:SenderName"], _config["EmailSettings:SenderEmail"]));
            emailMessage.To.Add(new MailboxAddress(toEmail, toEmail));
            emailMessage.Subject = subject;

            emailMessage.Body = new TextPart("html") { Text = message };

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync(_config["EmailSettings:SmtpServer"], int.Parse(_config["EmailSettings:Port"]), MailKit.Security.SecureSocketOptions.StartTls);
                await client.AuthenticateAsync(_config["EmailSettings:SenderEmail"], _config["EmailSettings:Password"]);
                await client.SendAsync(emailMessage);
                await client.DisconnectAsync(true);
            }
        }
    }
}
