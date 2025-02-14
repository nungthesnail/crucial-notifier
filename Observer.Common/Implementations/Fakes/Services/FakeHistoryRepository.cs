using System.Globalization;
using Observer.Common.Interfaces.Services;
using Observer.Common.Models;

namespace Observer.Common.Implementations.Fakes.Services;

public class FakeHistoryRepository : IHistoryRepository
{
    private readonly List<WebPageContent> _storage = [
        new()
        {
            Hash = "71c9a36a6131f2c0eea65cc1aa9f6866",
            LastModified = DateTimeOffset.Parse("2020-09-17", CultureInfo.InvariantCulture)
        }
    ];
    
    public Task AddHistoryStampAsync(WebPageContent current, CancellationToken _)
    {
        _storage.Add(current);
        return Task.CompletedTask;
    }

    public WebPageContent? GetLastContent(CancellationToken stoppingToken)
    {
        return _storage.LastOrDefault();
    }

    public Task AddSendingEventAsync(bool sent, CancellationToken stoppingToken)
    {
        return Task.CompletedTask;
    }
}