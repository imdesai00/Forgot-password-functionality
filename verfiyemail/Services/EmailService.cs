using Microsoft.Extensions.Options;
using System.Net.Mail;
using System.Net;
using verfiyemail.Models;

namespace verfiyemail.Services
{

    public class EmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmailAsync(string to, string subject, string htmlMessage)
        {
            var emailConfig = _configuration.GetSection("SmtpSettings");
            var from = emailConfig["FromAddress"];
            var smtpHost = emailConfig["Server"];
            var smtpPort = int.Parse(emailConfig["Port"]);
            var smtpUser = emailConfig["Username"];
            var smtpPass = emailConfig["Password"];

            var email = new MailMessage(from, to)
            {
                Subject = subject,
                Body = htmlMessage,
                IsBodyHtml = true,
            };

            using var smtp = new SmtpClient(smtpHost, smtpPort)
            {
                Credentials = new NetworkCredential(smtpUser, smtpPass),
                EnableSsl = true,
            };

            try
            {
                await smtp.SendMailAsync(email);
            }
            catch (Exception ex)
            {
                // Log or handle exception
                throw;
            }
        }
    }
}
