namespace Crucial.Core.Interfaces.Dal;

public interface ITransactionManager : IAsyncDisposable
{
    Task BeginTransactionAsync();
    Task CommitTransactionAsync(bool saveChanges = true);
    Task RollbackTransactionAsync();
}
