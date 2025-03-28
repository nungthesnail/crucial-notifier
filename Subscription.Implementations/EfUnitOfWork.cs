using Microsoft.EntityFrameworkCore.Storage;
using Subscription.Core.Interfaces;
using Subscription.EntityFrameworkCore;
using Subscription.Implementations.Repositories;
using Subscription.Model.Exceptions;
using TransactionException = Subscription.Model.Exceptions.TransactionException;

namespace Subscription.Implementations;

public class EfUnitOfWork : IUnitOfWork, IAsyncDisposable
{
    private readonly ApplicationDbContext _dbContext;
    private IDbContextTransaction? _transactionScope;
    
    public ISubscriptionRepository SubscriptionRepository { get; }

    public EfUnitOfWork(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
        SubscriptionRepository = new EfSubscriptionRepository(dbContext);
    }

    public async Task BeginTransactionAsync()
    {
        _transactionScope = await _dbContext.Database.BeginTransactionAsync();
    }

    public async Task CommitTransactionAsync()
    {
        try
        {
            await _dbContext.SaveChangesAsync();
            if (_transactionScope is null)
                throw new TransactionNotStartedException();
            
            await _transactionScope.CommitAsync();
            await DisposeTransactionScopeAsync();
        }
        catch (TransactionNotStartedException)
        {
            throw;
        }
        catch (Exception exc)
        {
            if (_transactionScope is null)
                throw new TransactionException("Transaction hasn't been begun");

            await RollbackTransactionAsync();
            throw new SubscriptionFailedException("Something went wrong while committing transaction", exc);
        }
    }

    private async Task DisposeTransactionScopeAsync()
    {
        if (_transactionScope is null)
            return;
        
        await _transactionScope.DisposeAsync();
        _transactionScope = null;
    }

    public async Task RollbackTransactionAsync()
    {
        if (_transactionScope is null)
            throw new TransactionException("Transaction hasn't been begun");
            
        await _transactionScope.RollbackAsync();
        await DisposeTransactionScopeAsync();
    }

    public async ValueTask DisposeAsync()
    {
        if (_transactionScope is not null)
            await _transactionScope.DisposeAsync();
        GC.SuppressFinalize(this);
    }
}
