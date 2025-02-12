using Microsoft.Extensions.DependencyInjection;
using Notifier.Common.Interfaces.BasicNotification;
using Notifier.Common.Models.BasicNotification;
using Notifier.Common.Models.Notification;

namespace Notifier.Common.Implementations.BasicNotification;

public class BasicNotificationSenderResolver : IBasicNotificationSenderResolver
{
    private readonly IServiceProvider _serviceProvider;

    public BasicNotificationSenderResolver(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }
    
    public IBasicNotificationSender Resolve(NotificationRecipientType recipientType)
    {
        var senderKey = RecipientTypeToSenderKey();
        return _serviceProvider.GetKeyedService<IBasicNotificationSender>(senderKey)
               ?? throw new InvalidOperationException($"There is no basic notification sender with key {senderKey}.");
        
        string RecipientTypeToSenderKey()
        {
            return recipientType switch
            {
                NotificationRecipientType.Email => BasicNotificationSenderTypes.Email,
                _ => throw new ArgumentOutOfRangeException($"There is no sender for type {recipientType}.")
            };
        }
    }
}
