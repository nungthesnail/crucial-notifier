using Observer.Common.Models.Notifier;

namespace Observer.Common.Interfaces.Services;

public interface IRecipientsProvider
{
    Task<IEnumerable<NotificationRecipient>> GetRecipientsAsync(CancellationToken stoppingToken);
}