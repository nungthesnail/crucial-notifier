namespace Observer.Common.Models.Notifier;

public class NotificationMessage : BaseMessage
{
    public override string MessageType => "BasicNotification";
    public IEnumerable<NotificationRecipient>? Recipients { get; set; }
    public string? Content { get; set; }
}