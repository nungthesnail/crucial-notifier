using Crucial.Core.Models;

namespace Crucial.Core.Interfaces.Managers;

public interface ICourseManager
{
    Task<Guid> CreateCourseAsync(CourseDto courseData);
    Task<CourseDto?> GetCourseByIdAsync(Guid courseId);
    Task<IEnumerable<CourseDto?>> GetCourseByNameAsync(string courseName);
    Task<IEnumerable<CourseDto>> GetAllCoursesAsync();
    Task EditCourseAsync(CourseDto course);
    Task LeaveCourseAsync(Guid courseId);
}
