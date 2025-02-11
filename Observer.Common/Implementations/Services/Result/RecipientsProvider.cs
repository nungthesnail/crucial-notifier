using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Observer.Common.Interfaces.Services;
using Observer.Common.Models;
using Observer.Common.Models.Notifier;
using Observer.EntityFramework;

namespace Observer.Common.Implementations.Services.Result;

public class RecipientsProvider : IRecipientsProvider
{
    private readonly AppDbContext _dbContext;
    
    public RecipientsProvider(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<NotificationRecipient>> GetRecipientsAsync(CancellationToken stoppingToken)
    {
        var dbRecipients = await _dbContext
            .Recipients
            .Where(static x => x.Active)
            .ToListAsync(stoppingToken);

        return dbRecipients
            .Select(
                static x => new NotificationRecipient
                {
                    Identifier = x.Identifier,
                    Type = (NotificationRecipientType)x.Type
                });
    }
}