using Crucial.Core.Exceptions;
using Crucial.Core.Implementations;
using Crucial.Core.Interfaces.Dal;
using Crucial.Core.Models;

namespace Crucial.Core.Tests.Managers;

[TestFixture]
public class CourseManagerTests
{
    private readonly IUnitOfWork _uowMock = null!;
    private readonly List<CourseDto> _coursesStorage = [];
    
    [Test]
    public async Task TestCreateCourseAsync_InputIsNonExistentCourse_ResultIsItsIdAndAddedCourseToStorage_Async()
    {
        var courseId = Guid.NewGuid();
        try
        {
            // Arrange
            var course = new CourseDto(
                CourseId: courseId,
                Name: "Test Course",
                Description: null,
                TotalLessonsCount: 10);

            var manager = new CourseManager(_uowMock);

            // Act
            await manager.CreateCourseAsync(course);
            var existingCourse = _coursesStorage.First(x => x.CourseId == courseId);

            // Assert
            Assert.That(existingCourse, Is.EqualTo(course));
        }
        finally
        {
            // -- Clear --
            _coursesStorage.RemoveAll(x => x.CourseId == courseId);
        }
    }

    [Test]
    public async Task TestCreateCourseAsync_InputIsCourseWithIdAlreadyExists_ThrowsCourseAlreadyExistsException_Async()
    {
        var courseId = Guid.NewGuid();
        try
        {
            // Arrange
            var course1 = new CourseDto(
                CourseId: courseId,
                Name: "Test Course 1",
                Description: null,
                TotalLessonsCount: 10);
            var course2 = new CourseDto(
                CourseId: courseId,
                Name: "Test Course 2",
                Description: null,
                TotalLessonsCount: 5);
            _coursesStorage.Add(course1);

            var manager = new CourseManager(_uowMock);

            // Act
            AsyncTestDelegate throws = () => manager.CreateCourseAsync(course2);

            // Assert
            await Assert.ThatAsync(throws, Throws.InstanceOf<CourseAlreadyExistsException>());
        }
        finally
        {
            _coursesStorage.RemoveAll(x => x.CourseId == courseId);
        }
    }
}
