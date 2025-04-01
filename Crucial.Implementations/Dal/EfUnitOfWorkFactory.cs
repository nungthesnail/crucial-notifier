using Crucial.Core.Implementations;
using Crucial.Core.Interfaces.Dal;
using Crucial.EntityFrameworkCore;

namespace Crucial.Implementations.Dal;

public class EfUnitOfWorkFactory : BaseUnitOfWorkFactory
{
    private readonly ApplicationDbContext _dbContext;

    public EfUnitOfWorkFactory(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public override IUnitOfWork Create()
    {
        var transactionManager = new EfTransactionManager(_dbContext);
        var courseRepository = new EfCourseRepository(_dbContext);
        return CreateInstanceInternal(transactionManager, courseRepository);
    }
}
