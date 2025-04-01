using Crucial.Core.Exceptions;
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
    
    public async Task<Guid> CreateCourseAsync(CourseDto courseData)
    {
        try
        {
            await _uow.BeginTransactionAsync();
            await _uow.CourseRepository.AddAsync(courseData);
            await _uow.CommitTransactionAsync();
            return courseData.Id;
        }
        catch (TransactionException exc)
        {
            throw new InternalCourseException(exc.Message, exc.InnerException);
        }
    }

    public async Task<CourseDto?> GetCourseByIdAsync(Guid courseId)
    {
        await _uow.BeginTransactionAsync();
        var result = await _uow.CourseRepository.GetAsync(courseId);
        await _uow.CommitTransactionAsync(saveChanges: false);
        return result;
        
    }

    public async Task<IEnumerable<CourseDto?>> GetCourseByNameAsync(string courseName)
    {
        await _uow.BeginTransactionAsync();
        var result = await _uow.CourseRepository.GetByNameAsync(courseName);
        await _uow.CommitTransactionAsync(saveChanges: false);
        return [result]; // In future versions here will be a text search
    }

    public async Task<IEnumerable<CourseDto>> GetAllCoursesAsync()
    {
        await _uow.BeginTransactionAsync();
        var result = await _uow.CourseRepository.GetAllAsync();
        await _uow.CommitTransactionAsync(saveChanges: false);
        return result;
    }

    public async Task EditCourseAsync(CourseDto course)
    {
        try
        {
            await _uow.BeginTransactionAsync();
            await _uow.CourseRepository.UpdateAsync(course);
            await _uow.CommitTransactionAsync();
        }
        catch (TransactionException exc)
        {
            throw new InternalCourseException(exc.Message, exc.InnerException);
        }
    }

    public async Task LeaveCourseAsync(Guid courseId)
    {
        try
        {
            await _uow.BeginTransactionAsync();
            await _uow.CourseRepository.DeleteAsync(courseId);
            await _uow.CommitTransactionAsync();
        }
        catch (TransactionException exc)
        {
            throw new InternalCourseException(exc.Message, exc.InnerException);
        }
    }
}
