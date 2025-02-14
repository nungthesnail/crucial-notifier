namespace Observer.EntityFramework.Models;

public class NotificationSendingEvent
{
    public int Id { get; set; }
    public DateTimeOffset? Timestamp { get; set; }
    public bool Sent { get; set; }
}