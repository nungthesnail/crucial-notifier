using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Observer.EntityFramework;

public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        optionsBuilder.UseNpgsql("Server=localhost;Port=5432;Database=crucial_notifier;Timeout=1000;CommandTimeout=1000;User Id=postgres;Password=123;ApplicationName=Observer;Pooling=true;MinPoolSize=1;MaxPoolSize=100;");
        return new AppDbContext(optionsBuilder.Options);
    }
}
