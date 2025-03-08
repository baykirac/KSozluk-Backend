using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;

namespace UserApi.Models;

public class EmailMessage
{
    private readonly MailMessage _MailMessage;

    public MailMessage MailMessage { get => _MailMessage; }

    public List<string> ToAddresses { get; init; }
    public string FromAddress { get; init; }
    public string Subject { get; init; }
    public string Body { get; init; }
    public bool IsBodyHtml { get; init; }

    public EmailMessage(string _fromAddress,
        IEnumerable<string> _toAddresses,
        string _subject,
        string _body, bool
        _isBodyHtml = false)
    {
        _MailMessage = new MailMessage
        {
            From = new MailAddress(_fromAddress),

            Subject = _subject,

            Body = _body,

            IsBodyHtml = _isBodyHtml
        };

        _toAddresses = _toAddresses.Select(x =>
        {
            _MailMessage.To.Add(x);

            return x;
        });

        FromAddress = _fromAddress;

        Subject = _subject;

        Body = _body;

        IsBodyHtml = _isBodyHtml;
    }

    public EmailMessage(string _fromAddress,
        string _toAddress,
        string _subject,
        string _body,
        bool _isBodyHtml = false)
    {
        _MailMessage = new MailMessage(_fromAddress, _toAddress, _subject, _body)
        {
            IsBodyHtml = _isBodyHtml
        };

        FromAddress = _fromAddress;

        ToAddresses = new List<string> { _toAddress };

        Subject = _subject;

        Body = _body;

        IsBodyHtml = _isBodyHtml;
    }

    public EmailMessage(string _fromAddress,
        string _toAddress,
        string _subject,
        AlternateView _alternateView)
    {
        _MailMessage = new MailMessage(_fromAddress, _toAddress, _subject, "")
        {
            IsBodyHtml = true,
        };

        _MailMessage.AlternateViews.Add(_alternateView);

        FromAddress = _fromAddress;

        ToAddresses = new List<string> { _toAddress };

        Subject = _subject;

        IsBodyHtml = true;
    }
}