using Observer.Common.Models;

namespace Observer.Common.Interfaces.Services;

public interface IContentProvider
{
    Task<WebPageContent> GetContentAsync(CancellationToken stoppingToken);
}
