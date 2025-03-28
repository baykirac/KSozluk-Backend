using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace UserApi.Models;

public class EmailTool
{
    public EmailConfiguration Configuration { get; init; }

    public EmailTool(IOptions<EmailConfiguration> _config)
    {

        Configuration = _config.Value;

    }


    public void SendEmail(EmailMessage _message)
    {
        using var _smtpClient = new SmtpClient
        {

            Host = Configuration.SmtpServer,

            Port = Configuration.Port,

        };

        _smtpClient.UseDefaultCredentials = false;

        _smtpClient.Credentials = new NetworkCredential(
            Configuration.UserName, Configuration.Password);


        //office365 için
        _smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;

        _smtpClient.EnableSsl = true;

        _smtpClient.Send(_message.MailMessage);
    }
    public async Task SendEmailAsync(EmailMessage _message)
    {
        using var _smtpClient = new SmtpClient
        {

            Host = Configuration.SmtpServer,

            Port = Configuration.Port,
            
        };

        _smtpClient.UseDefaultCredentials = false;

        _smtpClient.Credentials = new NetworkCredential(
            Configuration.UserName, Configuration.Password);

        _smtpClient.EnableSsl = true;

        await _smtpClient.SendMailAsync(_message.MailMessage);
    }
}