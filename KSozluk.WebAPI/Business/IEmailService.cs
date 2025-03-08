using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;

namespace KSozluk.WebAPI.Business
{
    public interface IEmailService
    {
        Task SendEmailAsync(string to, string subject, string body);
    }
}
