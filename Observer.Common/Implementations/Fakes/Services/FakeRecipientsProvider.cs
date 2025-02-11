using Observer.Common.Interfaces.Services;
using Observer.Common.Models;
using Observer.Common.Models.Notifier;

namespace Observer.Common.Implementations.Fakes.Services;

public class FakeRecipientsProvider : IRecipientsProvider
{
    public Task<IEnumerable<NotificationRecipient>> GetRecipientsAsync(CancellationToken stoppingToken)
    {
        var recipients = new List<NotificationRecipient>()
        {
            new()
            {
                Identifier = "nungthesnail@mail.ru",
                Type = NotificationRecipientType.Email
            }
        };
        
        return Task.FromResult(recipients.AsEnumerable());
    }
}