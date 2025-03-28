using Microsoft.EntityFrameworkCore;
using Subscription.EntityFrameworkCore;

namespace Tests.Common.Implementations;

public abstract class EfDependentTestSuite<TDbContext, TDbContextFactory> : IAsyncDisposable
    where TDbContext : DbContext
    where TDbContextFactory : IAppDbContextFactory<TDbContext>, new()
{
    protected readonly TDbContext AppDbContext;
    
    protected EfDependentTestSuite(DbContextOptions<TDbContext> options)
    {
        var dbContextFactory = new TDbContextFactory();
        AppDbContext = dbContextFactory.Create(options);
    }

    public virtual async ValueTask DisposeAsync()
    {
        await AppDbContext.DisposeAsync();
        GC.SuppressFinalize(this);
    }
}
