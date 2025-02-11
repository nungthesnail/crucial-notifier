namespace Observer.Common.Models.Notifier;

public class NotificationMessage
{
    public IEnumerable<NotificationRecipient>? Recipients { get; set; }
    public string? Content { get; set; }
}