using Crucial.Core.Interfaces.Dal;

namespace Crucial.Core.Implementations;

public abstract class BaseUnitOfWorkFactory : IUnitOfWorkFactory
{
    public abstract IUnitOfWork Create();
    protected static IUnitOfWork CreateInstanceInternal(ITransactionManager transactionManager,
        ICourseRepository courseRepository) => new UnitOfWork(transactionManager, courseRepository);
}
