using Crucial.Core.Interfaces.Dal;
using Crucial.Core.Interfaces.Managers;
using Crucial.Core.Models;

namespace Crucial.Core.Implementations;

public class CourseManager : ICourseManager
{
    private readonly IUnitOfWork _uow;

    public CourseManager(IUnitOfWork uow)
    {
        _uow = uow;
    }
    
    public Task<Guid> CreateCourseAsync(CourseDto courseSettings)
    {
        throw new NotImplementedException();
    }

    public Task<CourseDto?> GetCourseByIdAsync(Guid courseId)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<CourseDto?>> GetCourseByNameAsync(string courseName)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<CourseDto>> GetAllCoursesAsync()
    {
        throw new NotImplementedException();
    }

    public Task EditCourseAsync(CourseDto course)
    {
        throw new NotImplementedException();
    }

    public Task LeaveCourseAsync(Guid courseId)
    {
        throw new NotImplementedException();
    }
}
