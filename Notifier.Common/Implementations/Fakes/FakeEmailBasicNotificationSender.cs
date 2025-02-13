using Notifier.Common.Interfaces.BasicNotification;

namespace Notifier.Common.Implementations.Fakes;

public class FakeEmailBasicNotificationSender : IBasicNotificationSender
{
    public Task SendAsync(IEnumerable<string> identifiers, string content)
    {
        throw new NotImplementedException();
    }
}
