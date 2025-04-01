using Crucial.Core.Exceptions;
using Crucial.Core.Interfaces.Dal;
using Crucial.Core.Models;
using Crucial.EntityFrameworkCore;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Crucial.Implementations.Dal;

public class EfCourseRepository : ICourseRepository
{
    private readonly ApplicationDbContext _dbContext;

    public EfCourseRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync(CourseDto course)
    {
        var dbEntity = course.Adapt<CourseModel>();

        if (await _dbContext.Courses.AnyAsync(c => c.Name == course.Name || c.Id == course.Id))
            throw new CourseAlreadyExistsException("Course with the same name or id already exists");
        
        await _dbContext.Courses.AddAsync(dbEntity);
    }

    public async Task<CourseDto?> GetAsync(Guid key)
    {
        return (await _dbContext.Courses.FirstOrDefaultAsync(c => c.Id == key))
            .Adapt<CourseDto>();
    }

    public async Task<CourseDto?> GetByNameAsync(string name)
    {
        return (await _dbContext.Courses.FirstOrDefaultAsync(c => c.Name == name))
            .Adapt<CourseDto>();
    }

    public async Task<IEnumerable<CourseDto>> GetAllAsync()
    {
        return (await _dbContext.Courses.ToListAsync())
            .Adapt<List<CourseDto>>();
    }

    public async Task UpdateAsync(CourseDto course)
    {
        var entity = await _dbContext.Courses.FirstOrDefaultAsync(x => x.Id == course.Id);
        if (entity is null)
            throw new CourseDoesntExistsException($"Course with id {course.Id} doesn't exists");
        
        entity.Name = course.Name;
        entity.Description = course.Description;
        entity.TotalLessonsCount = course.TotalLessonsCount;
        entity.LessonsPassedCount = course.LessonsPassedCount;
        
        _dbContext.Courses.Update(entity);
    }

    public async Task DeleteAsync(Guid key)
    {
        var entity = await _dbContext.Courses.FirstOrDefaultAsync(x => x.Id == key);
        if (entity is null)
            return;
        
        _dbContext.Courses.Remove(entity);
    }
}
