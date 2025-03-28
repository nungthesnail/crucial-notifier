using Crucial.Core.Interfaces.Dal;
using Crucial.Core.Interfaces.Managers;

namespace Crucial.Core.Implementations;

public class LessonManager : ILessonManager
{
    private readonly IUnitOfWork _uow;

    public LessonManager(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public Task AddPassedLessonsToCourseAsync(Guid courseId, int count)
    {
        throw new NotImplementedException();
    }

    public Task<int> CalculateTotalLessonsCountAsync()
    {
        throw new NotImplementedException();
    }

    public Task<IDictionary<Guid, int>> CalculateLessonsCountByAllCoursesAsync()
    {
        throw new NotImplementedException();
    }
}
