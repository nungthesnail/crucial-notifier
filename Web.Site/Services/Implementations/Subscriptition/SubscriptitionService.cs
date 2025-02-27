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
}
