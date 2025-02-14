using Observer.Common.Interfaces.Services;
using Observer.Common.Models;
using Observer.EntityFramework;
using Observer.EntityFramework.Models;

namespace Observer.Common.Implementations.Services;

public sealed class HistoryRepository : IHistoryRepository
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
        await _dbContext.SaveChangesAsync(stoppingToken);
    }

    public async Task AddSendingEventAsync(bool sent, CancellationToken stoppingToken)
    {
        var dbModel = new NotificationSendingEvent
        {
            Sent = sent,
            Timestamp = DateTimeOffset.Now
        };
        
        await _dbContext.NotificationSendingEvents.AddAsync(dbModel, CancellationToken.None);
        await _dbContext.SaveChangesAsync(stoppingToken);
    }
}
