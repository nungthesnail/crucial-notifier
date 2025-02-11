namespace Observer.Common.Models.Notifier;

public class NotifierTask
{
    public NotificationData? Data { get; set; }
    public IEnumerable<NotificationRecipient> Recipients { get; set; } = new List<NotificationRecipient>();
}
