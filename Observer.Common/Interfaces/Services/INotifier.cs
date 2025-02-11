using Observer.Common.Models.Notifier;

namespace Observer.Common.Interfaces.Services;

public interface INotifier
{
    Task SendAsync(NotifierTask task, CancellationToken stoppingToken);
}