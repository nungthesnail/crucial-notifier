using System.Globalization;
using Crucial.Core.Models;
using Mapster;

namespace Crucial.EntityFrameworkCore.Tests;

[TestFixture]
public class MappingTests
{
    [Test]
    public void TestAdaptDto2Dal_LessonsIsNotEmpty_ReturnsCorrectDalModel()
    {
        // Arrange
        const string name = "Course 1";
        const string? description = "Some description";
        const int totalLessonsCount = 10;
        var lessonDate = DateTimeOffset.Parse("2025-04-05 23:11:00", CultureInfo.InvariantCulture);
        var courseId = Guid.Parse("c6a784f2-2ee0-4000-ac40-9fb7c2086753");
        var lessonId = Guid.Parse("c6fd43de-e5fd-477a-b37e-0c690e9fcb6a");
        
        var dto = new CourseDto(
            id: courseId,
            name: name,
            description: description,
            totalLessonsCount: totalLessonsCount,
            lessons: [
                new LessonDto
                {
                    Id = lessonId,
                    Date = lessonDate
                }
            ]);
        
        // Act
        var dal = dto.Adapt<CourseModel>();
        var firstLesson = dal.Lessons.First();
        
        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(dal.Id, Is.EqualTo(courseId));
            Assert.That(dal.Name, Is.EqualTo(name));
            Assert.That(dal.Description, Is.EqualTo(description));
            Assert.That(dal.TotalLessonsCount, Is.EqualTo(totalLessonsCount));
            Assert.That(firstLesson.Id, Is.EqualTo(lessonId));
            Assert.That(firstLesson.Date, Is.EqualTo(lessonDate));
        });
    }
}
