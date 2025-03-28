using Microsoft.EntityFrameworkCore;
using Subscription.EntityFrameworkCore;
using Subscription.Model;
using Tests.Common.Implementations;

namespace Subscription.Implementations.Tests;

public sealed class EfUnitOfWorkTests()
    : EfDependentTestSuite<ApplicationDbContext, ApplicationDbContextFactory>(Options)
{
    private const string TestConnectionString = "Server=localhost;Port=5432;Database=subscription_test_db;Timeout=1000;" +
                                                "CommandTimeout=1000;User Id=postgres;Password=123;" +
                                                "ApplicationName=SubscriptionApi;Pooling=true;MinPoolSize=1;" +
                                                "MaxPoolSize=100;";
    
    private static readonly DbContextOptions<ApplicationDbContext> Options;

    [Test]
    public async Task TestCommonFunctionality_Async()
    {
        var subsId = Guid.NewGuid();
        var userId = Guid.NewGuid().ToString();
        var uow = new EfUnitOfWork(AppDbContext);
        
        // -- Creating record --
        await uow.BeginTransactionAsync();
        
        await uow.SubscriptionRepository.AddAsync(new SubscriptionModel
        {
            Id = subsId,
            Email = "test@example.com",
            UserId = userId,
            Active = true
        });

        await uow.CommitTransactionAsync();
        
        // -- Getting record --

        await uow.BeginTransactionAsync();
        var record = await uow.SubscriptionRepository.GetByUserIdAsync(userId);
        await uow.CommitTransactionAsync();
        
        Assert.That(record, Is.Not.Null);
        
        // -- Deleting record --
        
        await uow.BeginTransactionAsync();
        await uow.SubscriptionRepository.DeleteAsync(record.Id);
        await uow.CommitTransactionAsync();
        
        // -- Checking record deleted --
        
        await uow.BeginTransactionAsync();
        var deletedRecord = await uow.SubscriptionRepository.GetByUserIdAsync(userId);
        await uow.CommitTransactionAsync();
        
        Assert.That(deletedRecord, Is.Null);
        
        // -- Disposing --

        await uow.DisposeAsync();
    }
    
    [TearDown]
    public async Task TearDownAsync()
    {
        await DisposeAsync();
    }
    
    static EfUnitOfWorkTests()
    {
        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
        optionsBuilder.UseNpgsql(TestConnectionString);
        Options = optionsBuilder.Options;
    }
}
