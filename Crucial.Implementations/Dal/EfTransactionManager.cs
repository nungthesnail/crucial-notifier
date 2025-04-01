using Crucial.Core.Exceptions;
using Crucial.Core.Interfaces.Dal;
using Crucial.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Crucial.Implementations.Dal;

public class EfTransactionManager : ITransactionManager
{
    private readonly ApplicationDbContext _dbContext;
    private IDbContextTransaction? _transactionScope;

    public EfTransactionManager(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task BeginTransactionAsync()
    {
        if (_transactionScope is not null)
            throw new InvalidOperationException("Transaction already started");
        
        _transactionScope = await _dbContext.Database.BeginTransactionAsync();
    }

    public async Task CommitTransactionAsync(bool saveChanges = true)
    {
        try
        {
            if (saveChanges)
                await _dbContext.SaveChangesAsync();
            if (_transactionScope is null)
                throw new TransactionNotStartedException("Transaction not started");
            await _transactionScope.CommitAsync();
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
            throw new TransactionFailedException("Something went wrong while committing transaction", exc);
        }
        finally
        {
            await DisposeTransactionScopeAsync();
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
            throw new InvalidOperationException("Transaction not started");
        
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
