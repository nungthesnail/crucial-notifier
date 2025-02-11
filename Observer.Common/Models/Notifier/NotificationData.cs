namespace Observer.Common.Models.Notifier;

public class NotificationData
{
    public bool HashUpdated { get; set; }
    public bool ModifiedTimestampUpdated { get; set; }
    public DateTimeOffset? ModifiedTimestamp { get; set; }
}
