using Crucial.Core.Exceptions;
using Crucial.Core.Interfaces.Dal;
using Crucial.Core.Interfaces.Managers;
using Crucial.Core.Models;

namespace Crucial.Core.Implementations;

public class LessonManager : ILessonManager
{
    private readonly IUnitOfWork _uow;

    public LessonManager(IUnitOfWork uow)
    {
        _uow = uow;
    }
    
    public async Task AddPassedLessonsToCourseAsync(Guid courseId, IEnumerable<LessonDto> lessons)
    {
        try
        {
            await _uow.BeginTransactionAsync();
            
            var course = await _uow.CourseRepository.GetAsync(courseId);
            if (course is null)
                throw new CourseDoesntExistsException($"Course with id {courseId} doesn't exist");
            
            course.Lessons.Clear();
            course.Lessons.AddRange(lessons);
            await _uow.CourseRepository.UpdateAsync(course);
            
            await _uow.CommitTransactionAsync();
        }
        catch (TransactionException exc)
        {
            throw new InternalCourseException(exc.Message, exc.InnerException);
        }
    }

    public async Task<int> CalculateTotalLessonsCountAsync()
    {
        try
        {
            await _uow.BeginTransactionAsync();
            
            var allCourses = await _uow.CourseRepository.GetAllAsync();
            var totalCount = allCourses
                .Select(static x => x.TotalLessonsCount)
                .Sum();
            
            await _uow.CommitTransactionAsync(saveChanges: false);
            return totalCount;
        }
        catch (TransactionException exc)
        {
            throw new InternalCourseException(exc.Message, exc.InnerException);
        }
    }

    public async Task<int> CalculateAllPassedLessonsCountAsync()
    {
        try
        {
            await _uow.BeginTransactionAsync();
            
            var allCourses = await _uow.CourseRepository.GetAllAsync();
            var result = allCourses
                .Select(static x => x.LessonsPassedCount)
                .Sum();
            
            await _uow.CommitTransactionAsync(saveChanges: false);
            return result;
        }
        catch (TransactionException exc)
        {
            throw new InternalCourseException(exc.Message, exc.InnerException);
        }
    }
}
