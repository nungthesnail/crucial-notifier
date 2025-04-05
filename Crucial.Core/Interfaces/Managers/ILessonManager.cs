using Crucial.Core.Models;

namespace Crucial.Core.Interfaces.Managers;

public interface ILessonManager
{
    Task AddPassedLessonsToCourseAsync(Guid courseId, IEnumerable<LessonDto> lessons);
    Task<int> CalculateTotalLessonsCountAsync();
    Task<int> CalculateAllPassedLessonsCountAsync();
}
