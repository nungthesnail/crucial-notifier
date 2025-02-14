using System.Text.Json;
using Microsoft.Extensions.Logging;
using Notifier.Common.Exceptions;
using Notifier.Common.Interfaces.BasicNotification;
using Notifier.Common.Interfaces.Message;
using Notifier.Common.Models.Notification;

namespace Notifier.Common.Implementations.Message;

public class BasicNotificationHandler : IMessageHandler
{
    private readonly ILogger<BasicNotificationHandler> _logger;
    private readonly IBasicNotificationSenderResolver _senderResolver;

    public BasicNotificationHandler(ILogger<BasicNotificationHandler> logger,
        IBasicNotificationSenderResolver senderResolver)
    {
        _logger = logger;
        _senderResolver = senderResolver;
    }
    
    public Task Handle(string message)
    {
        _logger.LogInformation("Handling basic notification...");
        var notification = JsonSerializer.Deserialize<NotificationMessage>(message)
                           ?? throw new BadJsonException("Can't deserialize basic notification");
        ArgumentNullException.ThrowIfNull(notification.Recipients);
        
        LogNotificationContent(notification);
        LogNotificationRecipients(notification);

        return SendNotificationAsync(
            notification.Recipients,
            notification.Content ?? throw new InvalidOperationException("Email content isn't specified."),
            NotificationRecipientType.Email); // Sending emails
    }

    private void LogNotificationContent(NotificationMessage notification)
        => _logger.LogTrace("Notification content: {content}", notification.Content);

    private void LogNotificationRecipients(NotificationMessage notification)
    {
        
        _logger.LogTrace("Notification recipients:");
        foreach (var recipient in notification.Recipients!)
        {
            _logger.LogTrace("Recipient: {identifier}, {type}", recipient.Identifier, recipient.Type);
        }
    }

    private Task SendNotificationAsync(IEnumerable<NotificationRecipient> recipients, string message,
        NotificationRecipientType recipientType)
    {
        var selectedRecipients = recipients
            .Where(static x => x is {Type: NotificationRecipientType.Email, Identifier: not null})
            .Select(static x => x.Identifier!);
        
        var sender = _senderResolver.Resolve(recipientType);
        return sender.SendAsync(selectedRecipients, message);
    }
}
