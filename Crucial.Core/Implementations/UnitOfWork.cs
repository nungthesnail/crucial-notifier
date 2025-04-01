using Crucial.Core.Interfaces.Dal;

namespace Crucial.Core.Implementations;

public class UnitOfWork : IUnitOfWork
{
    private readonly ITransactionManager _transactionManager;
    public ICourseRepository CourseRepository { get; }

    internal UnitOfWork(ITransactionManager transactionManager, ICourseRepository courseRepository)
    {
        _transactionManager = transactionManager;
        CourseRepository = courseRepository;
    }

    public Task BeginTransactionAsync() => _transactionManager.BeginTransactionAsync();
    public Task CommitTransactionAsync(bool saveChanges = true) => _transactionManager.CommitTransactionAsync();
    public Task RollbackTransactionAsync() => _transactionManager.RollbackTransactionAsync();

    public async ValueTask DisposeAsync()
    {
        await _transactionManager.DisposeAsync();
        GC.SuppressFinalize(this);
    }
}
