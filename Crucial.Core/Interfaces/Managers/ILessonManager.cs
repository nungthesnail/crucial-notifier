namespace Crucial.Core.Interfaces.Managers;

public interface ILessonManager
{
    Task AddPassedLessonsToCourseAsync(Guid courseId, int count);
    Task<int> CalculateTotalLessonsCountAsync();
    Task<IDictionary<Guid, int>> CalculateLessonsCountByAllCoursesAsync();
}
