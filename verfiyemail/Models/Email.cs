using Microsoft.Extensions.Options;
using System.Net.Mail;
using System.Net;

namespace verfiyemail.Models
{
    public interface IEmailService
    {
        Task SendEmailAsync(string toEmail, string subject, string body);
    }

    public class Email : IEmailService
    {
        private readonly SmtpSettings _smtpSettings;

        public Email(SmtpSettings smtpSettings)
        {
            _smtpSettings = smtpSettings;
        }
        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            using (var client = new SmtpClient(_smtpSettings.Server, _smtpSettings.Port))
            {
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential(_smtpSettings.Username, _smtpSettings.Password);
                client.EnableSsl = true;

                var message = new MailMessage
                {
                    From = new MailAddress(_smtpSettings.FromAddress, _smtpSettings.DisplayName),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true
                };

                message.To.Add(toEmail);

                await client.SendMailAsync(message);
            }
        }
    }
}
