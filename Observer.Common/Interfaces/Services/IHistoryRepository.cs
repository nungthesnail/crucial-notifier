using Observer.Common.Models;

namespace Observer.Common.Interfaces.Services;

public interface IHistoryRepository
{
    WebPageContent? GetLastContent(CancellationToken stoppingToken);
    
    Task AddHistoryStampAsync(WebPageContent current, CancellationToken stoppingToken);
}