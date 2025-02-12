namespace Notifier.Common.Interfaces.BasicNotification;

public interface IBasicNotificationSender
{
    Task SendAsync(IEnumerable<string> identifiers, string content);
}