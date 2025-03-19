using Subscription.Model;

namespace Subscription.Core.Interfaces;

public interface ISubscriptionManager
{
    Task SubscribeAsync(string userId, string email);
    Task ChangeSubscriptionEmailAsync(string userId, string newEmail);
    Task DisableSubscriptionAsync(string userId);
    Task EnableSubscriptionAsync(string userId);
    Task DeleteSubscriptionAsync(string userId);
    Task<IEnumerable<SubscriptionModel>> GetActiveSubscriptionsAsync();
}
