using Microsoft.EntityFrameworkCore;

namespace Subscription.EntityFrameworkCore;

public class ApplicationDbContextFactory : IAppDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext Create(DbContextOptions<ApplicationDbContext> options) => new(options);
}
