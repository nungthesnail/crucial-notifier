using Notifier.Common.Models.Notification;

namespace Notifier.Common.Interfaces.BasicNotification;

public interface IBasicNotificationSenderResolver
{
    IBasicNotificationSender Resolve(NotificationRecipientType recipientType);
}