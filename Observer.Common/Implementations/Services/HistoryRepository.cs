using Observer.Common.Interfaces.Services;
using Observer.Common.Models;
using Observer.EntityFramework;
using Observer.EntityFramework.Models;

namespace Observer.Common.Implementations.Services;

public class HistoryRepository : IHistoryRepository
{
    private readonly AppDbContext _dbContext;

    public HistoryRepository(AppDbContext dbContext)
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

    public async Task AddHistoryStampAsync(WebPageContent current, CancellationToken stoppingToken)
    {
        var dbModel = new HistoryStamp
        {
            Hash = current.Hash,
            LastModified = current.LastModified,
            InsertedAt = DateTimeOffset.Now
        };
        
        await _dbContext.HistoryStamps.AddAsync(dbModel, stoppingToken);
    }
}
