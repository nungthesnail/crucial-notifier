using Crucial.Core.Exceptions;
using Crucial.Core.Implementations;
using Crucial.Core.Interfaces.Dal;
using Crucial.Core.Models;
using Moq;

namespace Crucial.Core.Tests.Managers;

[TestFixture]
public class CourseManagerTests
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
    public async Task TestCreateCourseAsync_InputIsNonExistentCourse_ResultIsItsIdAndAddedCourseToStorage_Async()
    {
        var courseId = Guid.NewGuid();
        try
        {
            // Arrange
            var course = new CourseDto(
                Id: courseId,
                Name: "Test Course",
                Description: null,
                TotalLessonsCount: 10);

            var manager = new CourseManager(_uowMock);

            // Act
            await manager.CreateCourseAsync(course);
            var existingCourse = _coursesStorage.First(x => x.Id == courseId);

            // Assert
            Assert.That(existingCourse, Is.EqualTo(course));
        }
        finally
        {
            // -- Clear --
            _coursesStorage.RemoveAll(x => x.Id == courseId);
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
                Id: courseId,
                Name: "Test Course 1",
                Description: null,
                TotalLessonsCount: 10);
            var course2 = new CourseDto(
                Id: courseId,
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
            _coursesStorage.RemoveAll(x => x.Id == courseId);
        }
    }
}
