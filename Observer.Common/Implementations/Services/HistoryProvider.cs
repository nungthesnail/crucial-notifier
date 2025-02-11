using Observer.Common.Interfaces.Services;
using Observer.Common.Models;
using Observer.EntityFramework;

namespace Observer.Common.Implementations.Services;

public class HistoryProvider : IHistoryProvider
{
    private readonly AppDbContext _dbContext;

    public HistoryProvider(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public WebPageContent? GetLastContent(CancellationToken stoppingToken)
    {
        stoppingToken.ThrowIfCancellationRequested();
        
        var lastStamp = _dbContext
            .HistoryStamps
            .OrderBy(static x => x.InsertedAt)
            .LastOrDefault();

        if (lastStamp is null) return null;

        return new WebPageContent
        {
            Hash = lastStamp.Hash,
            LastModified = lastStamp.LastModified
        };
    }
}
