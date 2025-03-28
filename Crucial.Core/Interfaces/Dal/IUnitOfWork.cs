namespace Crucial.Core.Interfaces.Dal;

public interface IUnitOfWork : IAsyncDisposable
{
    ICourseRepository CourseRepository { get; }
    
    Task BeginTransactionAsync();
    Task CommitTransactionAsync();
    Task RollbackTransactionAsync();
}
