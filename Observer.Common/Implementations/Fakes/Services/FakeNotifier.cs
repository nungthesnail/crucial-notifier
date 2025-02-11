using Observer.Common.Interfaces.Services;
using Observer.Common.Models.Notifier;

namespace Observer.Common.Implementations.Fakes.Services;

public class FakeNotifier : INotifier
{
    public Task SendAsync(NotifierTask _, CancellationToken __)
    {
        return Task.CompletedTask;
    }
}