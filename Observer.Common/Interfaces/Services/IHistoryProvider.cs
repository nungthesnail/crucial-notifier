using Observer.Common.Models;

namespace Observer.Common.Interfaces.Services;

public interface IHistoryProvider
{
    WebPageContent? GetLastContent(CancellationToken stoppingToken);
}