using Observer.Common.Interfaces.Services;
using Observer.Common.Models;
using Observer.Common.Models.Notifier;

namespace Observer.Common.Implementations.Services.Result;

public class NotifierTaskBuilder : INotifierTaskBuilder
{
    private readonly IRecipientsProvider _recipientsProvider;

    public NotifierTaskBuilder(IRecipientsProvider recipientsProvider)
    {
        _recipientsProvider = recipientsProvider;
    }

    public async Task<NotifierTask> BuildAsync(ComparisonResult comparisonResult, CancellationToken stoppingToken)
    {
        var recipients = _recipientsProvider.GetRecipientsAsync(stoppingToken);
        var data = new NotificationData
        {
            HashUpdated = !comparisonResult.HashesIdentical,
            ModifiedTimestampUpdated = !comparisonResult.ModifiedTimestampsIdentical,
            ModifiedTimestamp = comparisonResult.LastModifiedTimestamp
        };

        return new NotifierTask
        {
            Data = data,
            Recipients = await recipients
        };
    }
}