namespace Subscription.Core.Interfaces;

public interface IUnitOfWork
{
    ISubscriptionRepository SubscriptionRepository { get; }
    Task BeginTransactionAsync();
    Task CommitTransactionAsync();
    Task RollbackTransactionAsync();
}
