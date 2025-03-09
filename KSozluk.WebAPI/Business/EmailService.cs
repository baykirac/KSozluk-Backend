using KSozluk.WebAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace KSozluk.WebAPI.Business
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

                var mailMessage = new MailMessage("kadirdaniisan@gmail.com", to, subject, body);

                mailMessage.IsBodyHtml = true;

                await _smtpClient.SendMailAsync(mailMessage);
                
            }
        }
    
}
