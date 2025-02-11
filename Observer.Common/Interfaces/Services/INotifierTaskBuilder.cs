using Observer.Common.Models;
using Observer.Common.Models.Notifier;

namespace Observer.Common.Interfaces.Services;

public interface INotifierTaskBuilder
{
    Task<NotifierTask> BuildAsync(ComparisonResult comparisonResult, CancellationToken stoppingToken);
}