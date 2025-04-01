using Crucial.Core.Models;

namespace Crucial.Core.Interfaces.Dal;

public interface ICourseRepository : IRepository<CourseDto, Guid>
{
    Task<CourseDto?> GetByNameAsync(string name);
}
