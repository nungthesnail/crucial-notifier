using Subscription.Core.Interfaces;
using Subscription.Model;
using Subscription.Model.Exceptions;

namespace Subscription.Core.Implementations;

public class SubscriptionManager(IUnitOfWork uow) : ISubscriptionManager
{
    private IUnitOfWork Uow { get; } = uow;

    public async Task<Guid> SubscribeAsync(string userId, string email)
    {
        Guid result;
        await Uow.BeginTransactionAsync();
        
        var repository = Uow.SubscriptionRepository;
        var recordByUserId = await repository.GetByUserIdAsync(userId);
        var recordByEmail = await repository.GetByEmailAsync(email);
        if (recordByEmail is not null && recordByEmail.UserId != userId)
        {
            await Uow.RollbackTransactionAsync();
            throw new SubscriptionBadDataException("Someone already have this email");
        }

        if (recordByUserId is not null)
        {
            recordByUserId.Email = email;
            recordByUserId.Active = true;
            await repository.UpdateAsync(recordByUserId);
            result = recordByUserId.Id;
        }
        else
        {
            result = Guid.NewGuid();
            await repository.AddAsync(new SubscriptionModel
            {
                Id = result,
                UserId = userId,
                Email = email,
                Active = true
            });
        }
        await Uow.CommitTransactionAsync();
        
        return result;
    }

    public async Task ChangeSubscriptionEmailAsync(string userId, string newEmail)
    {
        await Uow.BeginTransactionAsync();
        
        var repository = Uow.SubscriptionRepository;
        var recordByUserId = await repository.GetByUserIdAsync(userId);
        if (recordByUserId is null)
        {
            await Uow.RollbackTransactionAsync();
            throw new SubscriptionBadDataException("User doesn't exist");
        }

        var recordByEmail = await repository.GetByEmailAsync(newEmail);
        if (recordByEmail is not null && recordByEmail.UserId != userId)
        {
            await Uow.RollbackTransactionAsync();
            throw new SubscriptionBadDataException("Someone already have this email");
        }

        if (recordByEmail is not null && recordByEmail.Email == newEmail)
        {
            await Uow.CommitTransactionAsync();
            return;
        }

        recordByUserId.Email = newEmail;
        await repository.UpdateAsync(recordByUserId);
        
        await Uow.CommitTransactionAsync();
    }

    public async Task EnableSubscriptionAsync(string userId)
    {
        await Uow.BeginTransactionAsync();
        
        var repository = Uow.SubscriptionRepository;
        var recordByUserId = await repository.GetByUserIdAsync(userId);
        if (recordByUserId is null)
        {
            await Uow.RollbackTransactionAsync();
            throw new SubscriptionBadDataException("User doesn't exist");
        }

        if (!recordByUserId.Active)
        {
            recordByUserId.Active = true;
            await repository.UpdateAsync(recordByUserId);
        }
        
        await Uow.CommitTransactionAsync();
    }
    
    public async Task DisableSubscriptionAsync(string userId)
    {
        await Uow.BeginTransactionAsync();
        
        var repository = Uow.SubscriptionRepository;
        var recordByUserId = await repository.GetByUserIdAsync(userId);
        if (recordByUserId is null)
        {
            await Uow.RollbackTransactionAsync();
            throw new SubscriptionBadDataException("User doesn't exist");
        }

        if (recordByUserId.Active)
        {
            recordByUserId.Active = false;
            await repository.UpdateAsync(recordByUserId);
        }
        
        await Uow.CommitTransactionAsync();
    }

    public async Task DeleteSubscriptionAsync(string userId)
    {
        await Uow.BeginTransactionAsync();
        
        var repository = Uow.SubscriptionRepository;
        var recordByUserId = await repository.GetByUserIdAsync(userId);
        if (recordByUserId is null)
        {
            await Uow.RollbackTransactionAsync();
            throw new SubscriptionBadDataException("User doesn't exist");
        }

        await repository.DeleteAsync(recordByUserId.Id);
        
        await Uow.CommitTransactionAsync();
    }

    public async Task<IEnumerable<SubscriptionModel>> GetActiveSubscriptionsAsync()
    {
        await Uow.BeginTransactionAsync();
        
        var repository = Uow.SubscriptionRepository;
        var allRecords = await repository.GetAllAsync();
        var selected = allRecords.Where(static x => x.Active).ToList();
        
        await Uow.CommitTransactionAsync();
        
        return selected;
    }
}
