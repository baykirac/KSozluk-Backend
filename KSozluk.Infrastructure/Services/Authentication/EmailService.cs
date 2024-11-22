using KSozluk.Application.Services.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace KSozluk.Infrastructure.Services.Authentication
{
    public class EmailService : IEmailService
    {
      
            private readonly SmtpClient _smtpClient;

            public EmailService(SmtpClient smtpClient)
            {
                _smtpClient = smtpClient;
            }

            public async Task SendEmailAsync(string to, string subject, string body)
            {
                var mailMessage = new MailMessage("mytest.for.app.123456@gmail.com", to, subject, body);
                mailMessage.IsBodyHtml = true;

                await _smtpClient.SendMailAsync(mailMessage);
            }
        }
    
}
