using Microsoft.EntityFrameworkCore;

namespace Subscription.EntityFrameworkCore;

public interface IAppDbContextFactory<TDbContext>
    where TDbContext : DbContext
{
    TDbContext Create(DbContextOptions<TDbContext> options);
}
