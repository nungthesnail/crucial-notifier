using Microsoft.EntityFrameworkCore;
using Subscription.Core.Interfaces;
using Subscription.EntityFrameworkCore;
using Subscription.Model;

namespace Subscription.Implementations.Repositories;

public class EfSubscriptionRepository(ApplicationDbContext dbContext) : ISubscriptionRepository
{
    private ApplicationDbContext DbContext { get; } = dbContext;
    
    public async Task AddAsync(SubscriptionModel model)
    {
        await DbContext.Subscriptions.AddAsync(model);
        await DbContext.SaveChangesAsync();
    }

    public async Task<IEnumerable<SubscriptionModel>> GetAllAsync()
    {
        return await DbContext.Subscriptions.ToListAsync();
    }

    public Task<SubscriptionModel?> GetByIdAsync(Guid id)
    {
        return DbContext.Subscriptions.FirstOrDefaultAsync(x => x.Id == id);
    }

    public Task<SubscriptionModel?> GetByEmailAsync(string email)
    {
        return DbContext.Subscriptions.FirstOrDefaultAsync(x => x.Email == email);
    }

    public Task<SubscriptionModel?> GetByUserIdAsync(string userId)
    {
        return DbContext.Subscriptions.FirstOrDefaultAsync(x => x.UserId == userId);
    }

    public async Task UpdateAsync(SubscriptionModel model)
    {
        DbContext.Update(model);
        await DbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var subscription = await DbContext.Subscriptions.FirstOrDefaultAsync(x => x.Id == id);
        if (subscription is not null)
        {
            DbContext.Subscriptions.Remove(subscription);
            await DbContext.SaveChangesAsync();
        }
    }
}
