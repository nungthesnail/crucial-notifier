namespace Web.Site.Services.Interfaces;

public interface ISubscriptitionService
{
    Task<bool> IsUserSubscribedAsync(string? userName);
    Task SubscribeUserAsync(string? userName);
    Task<IEnumerable<string?>> GetSubscribersAsync();
    Task UnsubscribeUserAsync(string? userName);
}
