using Subscription.Model;

namespace Subscription.Core.Interfaces;

public interface ISubscriptionRepository : IRepository<SubscriptionModel, Guid>
{
    Task<SubscriptionModel?> GetByEmailAsync(string email);
    Task<SubscriptionModel?> GetByUserIdAsync(string userId);
}
