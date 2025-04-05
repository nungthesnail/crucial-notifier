using Crucial.Core.Exceptions;
using Crucial.Core.Implementations;
using Crucial.Core.Interfaces.Dal;
using Crucial.Core.Models;
using Moq;

namespace Crucial.Core.Tests.Managers;

public class LessonManagerTests
{
    private IUnitOfWork _uowMock = null!;
    private readonly List<CourseDto> _coursesStorage = [];

    [SetUp]
    public void Setup()
    {
        var uowMockSetup = new Mock<IUnitOfWork>();
        var coursesRepoMockSetup = new Mock<ICourseRepository>();
        
        // Repository mock setup
        coursesRepoMockSetup.Setup(static x => x.AddAsync(It.IsAny<CourseDto>()))
            .Returns((CourseDto course) =>
            {
                if (_coursesStorage.Any(x => x.Id == course.Id || x.Name == course.Name))
                    throw new CourseAlreadyExistsException();
                _coursesStorage.Add(course);
                return Task.CompletedTask;
            });
        coursesRepoMockSetup.Setup(static x => x.GetAllAsync())
            .ReturnsAsync(_coursesStorage.AsEnumerable());
        coursesRepoMockSetup.Setup(static x => x.GetByNameAsync(It.IsAny<string>()))
            .Returns((string name) => Task.FromResult(_coursesStorage.FirstOrDefault(x => x.Name == name)));
        coursesRepoMockSetup.Setup(static x => x.GetAsync(It.IsAny<Guid>()))
            .Returns((Guid id) => Task.FromResult(_coursesStorage.FirstOrDefault(x => x.Id == id)));
        coursesRepoMockSetup.Setup(static x => x.UpdateAsync(It.IsAny<CourseDto>()))
            .Returns((CourseDto course) =>
            {
                var existingEntry = _coursesStorage.FirstOrDefault(x => x.Id == course.Id);
                if (existingEntry is null)
                    throw new CourseDoesntExistsException();
                _coursesStorage.Remove(existingEntry);
                _coursesStorage.Add(course);
                
                return Task.CompletedTask;
            });
        coursesRepoMockSetup.Setup(static x => x.DeleteAsync(It.IsAny<Guid>()))
            .Returns((Guid id) =>
            {
                _coursesStorage.RemoveAll(x => x.Id == id);
                return Task.CompletedTask;
            });
        
        // Unit of work mock setup
        uowMockSetup.Setup(static x => x.BeginTransactionAsync()).Returns(Task.CompletedTask);
        uowMockSetup.Setup(static x => x.CommitTransactionAsync(It.IsAny<bool>())).Returns(Task.CompletedTask);
        uowMockSetup.Setup(static x => x.RollbackTransactionAsync()).Returns(Task.CompletedTask);

        uowMockSetup.Setup(static x => x.CourseRepository).Returns(coursesRepoMockSetup.Object);
        
        _uowMock = uowMockSetup.Object;
    }
    
    [Test]
    public async Task TestAddPassedLessonsToCourseAsync_InputIsCourseIdAnd10_Adds10ToLessonsCount_Async()
    {
        try
        {
            // Arrange
            const int addingPassedLessonsCount = 2;

            var courseId = Guid.Parse("20cebe1b-dc84-4fa2-8447-5e22840debb7");
            _coursesStorage.Add(new CourseDto(
                id: courseId,
                name: "Course 1",
                description: null,
                totalLessonsCount: 15));
            
            List<LessonDto> lessons =
            [
                new()
                {
                    Date = DateTime.Now.AddDays(-1),
                },
                new()
                {
                    Date = DateTime.Now.AddDays(-2),
                }
            ];

            var manager = new LessonManager(_uowMock);

            // Act
            await manager.AddPassedLessonsToCourseAsync(courseId, lessons);
            var courseEntity = _coursesStorage.First(x => x.Id == courseId);

            // Assert
            Assert.That(courseEntity.LessonsPassedCount, Is.EqualTo(addingPassedLessonsCount));
        }
        finally
        {
            _coursesStorage.Clear();
        }
    }

    [Test]
    public async Task TestCalculateTotalLessonsCountAsync_SumOfTotalLessonsCountIs25_Returns25_Async()
    {
        try
        {
            // Arrange
            _coursesStorage.AddRange(
                [
                    new CourseDto(
                        id: Guid.NewGuid(),
                        name: "Course 1",
                        description: null,
                        totalLessonsCount: 15),
                    new CourseDto(
                        id: Guid.NewGuid(),
                        name: "Course 2",
                        description: null,
                        totalLessonsCount: 3),
                    new CourseDto(
                        id: Guid.NewGuid(),
                        name: "Course 3",
                        description: null,
                        totalLessonsCount: 7)
                ]);
            const int expectedTotalLessonsCount = 25;
            var manager = new LessonManager(_uowMock);
            
            // Act
            var totalCount = await manager.CalculateTotalLessonsCountAsync();

            // Assert
            Assert.That(totalCount, Is.EqualTo(expectedTotalLessonsCount));
        }
        finally
        {
            _coursesStorage.Clear();
        }
    }

    [Test]
    public async Task TestCalculateTotalLessonsCountAsync_ThereIsNoCourses_Returns0_Async()
    {
        // Arrange
        const int expectedTotalLessonsCount = 0;
        _coursesStorage.Clear();
        var manager = new LessonManager(_uowMock);
        
        // Act
        var count = await manager.CalculateTotalLessonsCountAsync();
        
        // Assert
        Assert.That(count, Is.EqualTo(expectedTotalLessonsCount));
    }
    
    [Test]
    public async Task TestCalculateLessonsCountByAllCoursesAsync_SumOfPassedLessonsCountIs25_Returns25_Async()
    {
        try
        {
            // Arrange
            const int expectedResult = 25;
            
            var id1 = Guid.Parse("602ce7d3-ffda-4fcf-a47a-4303cf56d22c");
            var id2 = Guid.Parse("d965ee35-9560-487c-89f2-a8700ab2daca");
            var id3 = Guid.Parse("7b8250af-dac2-4df8-9cc7-5d496beb6825");
            _coursesStorage.AddRange(
            [
                new CourseDto(
                    id: id1,
                    name: "Course 1",
                    description: null,
                    totalLessonsCount: 20,
                    lessonsPassedCount: 10),
                new CourseDto(
                    id: id2,
                    name: "Course 2",
                    description: null,
                    totalLessonsCount: 20,
                    lessonsPassedCount: 11),
                new CourseDto(
                    id: id3,
                    name: "Course 3",
                    description: null,
                    totalLessonsCount: 20,
                    lessonsPassedCount: 4)
            ]);
            var manager = new LessonManager(_uowMock);
            
            // Act
            var totalCount = await manager.CalculateAllPassedLessonsCountAsync();

            // Assert
            Assert.That(totalCount, Is.EqualTo(expectedResult));
        }
        finally
        {
            _coursesStorage.Clear();
        }
    }
    
    
    [Test]
    public async Task TestCalculateLessonsCountByAllCoursesAsync_ThereIsNoCourses_Returns0_Async()
    {
        // Arrange
        const int expectedPassedLessonsCount = 0;
        _coursesStorage.Clear();
        var manager = new LessonManager(_uowMock);
        
        // Act
        var count = await manager.CalculateAllPassedLessonsCountAsync();
        
        // Assert
        Assert.That(count, Is.EqualTo(expectedPassedLessonsCount));
    }
}
