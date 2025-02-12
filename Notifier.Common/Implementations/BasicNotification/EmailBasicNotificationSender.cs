using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MailKit.Net.Smtp;
using MimeKit;
using MimeKit.Text;
using Notifier.Common.Interfaces.BasicNotification;
using Notifier.Common.Models.BasicNotification;

namespace Notifier.Common.Implementations.BasicNotification;

public class EmailBasicNotificationSender : IBasicNotificationSender
{
    private readonly EmailBasicNotificationSenderSettings _settings = new();
    private readonly ILogger<EmailBasicNotificationSender> _logger;

    public EmailBasicNotificationSender(ILogger<EmailBasicNotificationSender> logger, IConfiguration config)
    {
        config.GetSection("SmtpSettings").Bind(_settings);
        _logger = logger;
    }
    
    public async Task SendAsync(IEnumerable<string> identifiers, string content)
    {
        _logger.LogInformation("Sending email notifications...");
        using var message = BuildMessage(identifiers, content);
        using var client = new SmtpClient();
        
        await client.ConnectAsync(_settings.SmtpServerAddress, _settings.SmtpServerPort, true);
        await client.AuthenticateAsync(_settings.UserAddress, _settings.Password);
        await client.SendAsync(message);
        await client.DisconnectAsync(true);
        _logger.LogInformation("Email notifications sent.");
    }

    private MimeMessage BuildMessage(IEnumerable<string> identifiers, string content)
    {
        const string messageSubject = "Обновление расписания.";
        
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress(_settings.SenderName, _settings.UserAddress));
        
        AddRecipientsToMessage();

        message.Subject = messageSubject;
        message.Body = new TextPart(TextFormat.Plain)
        {
            Text = content
        };

        return message;

        void AddRecipientsToMessage()
        {
            foreach (var ind in identifiers)
            {
                _logger.LogTrace("Email notification recipient: {ind}", ind);
                message.To.Add(new MailboxAddress("", ind));
            }
        }
    }
}
