namespace Notifier.Common.Models.Notification;

public class NotificationMessage
{
    public IEnumerable<NotificationRecipient>? Recipients { get; set; }
    public string? Content { get; set; }
}