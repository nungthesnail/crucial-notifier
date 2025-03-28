using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Subscription.EntityFrameworkCore;

public class ApplicationDbContextDesignFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        ValidateArgs(args);
        var connectionString = args.First();
        
        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
        Console.WriteLine("Using connection string: {0}", connectionString);
        optionsBuilder.UseNpgsql(connectionString);
        return new ApplicationDbContext(optionsBuilder.Options);
    }

    private void ValidateArgs(string[] args)
    {
        if (args.Length > 0)
            return;
        
        Console.WriteLine("Invalid arguments: connection string is required.");
        throw new ArgumentException("Connection string is required.", nameof(args));
    }
}
