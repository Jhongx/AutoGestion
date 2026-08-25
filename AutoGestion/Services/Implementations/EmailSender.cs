using Microsoft.AspNetCore.Identity.UI.Services;
using System.Net;
using System.Net.Mail;

namespace AutoGestion.Services.Implementations
{
    public class EmailSender : IEmailSender
    {
        private readonly IConfiguration _configuration;

        public EmailSender(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var mailServer = _configuration["EmailSettings:MailServer"];
            var port = int.Parse(_configuration["EmailSettings:MailPort"]);

            // Configuración general
            var senderEmail = _configuration["EmailSettings:SenderEmail"];
            var senderName = _configuration["EmailSettings:SenderName"];

            // Credenciales leídas desde el archivo de configuración
            var smtpUser = _configuration["EmailSettings:SmtpUser"];
            var password = _configuration["EmailSettings:Password"];

            var client = new SmtpClient(mailServer, port)
            {
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(smtpUser, password), // <-- Aquí usa la variable del settings
                EnableSsl = true,
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(senderEmail!, senderName),
                Subject = subject,
                Body = htmlMessage,
                IsBodyHtml = true,
            };
            mailMessage.To.Add(email);

            return client.SendMailAsync(mailMessage);
        }
    }
}
