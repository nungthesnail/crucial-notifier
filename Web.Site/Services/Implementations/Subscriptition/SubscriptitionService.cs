using Microsoft.EntityFrameworkCore;
using Web.Site.Data;
using Web.Site.Services.Exceptions;
using Web.Site.Services.Interfaces;

namespace Web.Site.Services.Implementations.Subscriptition;

public class SubscriptitionService : ISubscriptitionService
{
    private readonly ApplicationDbContext _context;

    public SubscriptitionService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> IsUserSubscribedAsync(string? userName)
    {
        var record = await _context
            .Subscriptitions
            .Include(static x => x.User)
            .Where(x => x.User.UserName == userName)
            .FirstOrDefaultAsync();
        return record?.Active ?? throw new DataNotFoundException("User not found");
    }

    public async Task SubscribeUserAsync(string? userName)
    {
        var user = await _context.Users.FirstOrDefaultAsync(x => x.UserName == userName)
                   ?? throw new DataNotFoundException("User not found");
        var existingRecord = await _context
            .Subscriptitions
            .Where(x => x.UserId == user.Id)
            .FirstOrDefaultAsync();
        if (existingRecord is not null)
        {
            if (existingRecord.Active)
                return;
            
            existingRecord.Active = true;
            _context.Subscriptitions.Update(existingRecord);
            await _context.SaveChangesAsync();
            return;
        }
        
        var record = new Data.Models.Subscriptition
        {
            UserId = user.Id,
            Active = true
        };
        await _context.Subscriptitions.AddAsync(record);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<string?>> GetSubscribersAsync()
    {
        return await _context
            .Subscriptitions
            .Include(static x => x.User)
            .Where(static x => x.Active)
            .Select(static x => x.User.Email)
            .ToListAsync();
    }

    public async Task UnsubscribeUserAsync(string? userName)
    {
        var subscription = await _context
            .Subscriptitions
            .Where(x => x.User.UserName == userName)
            .FirstOrDefaultAsync();
        if (subscription is null)
            throw new DataNotFoundException("User not found");
        
        subscription.Active = false;
        _context.Subscriptitions.Update(subscription);
        await _context.SaveChangesAsync();
    }
}
