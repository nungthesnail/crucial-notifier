using Observer.Common.Implementations.Services;
using Observer.Common.Models;

namespace Observer.Common.Tests.Services;

public class DataComparerTests
{
    private readonly DataComparer _comparer = new();

    [Test]
    public void TestCompare_PreviousIsNull_ReturnsFullyDifferentResult()
    {
        // Arrange
        var current = new WebPageContent
        {
            Hash = string.Empty,
            LastModified = DateTimeOffset.MinValue
        };
        WebPageContent? previous = null;

        // Act
        var result = _comparer.Compare(current, previous);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.HashesIdentical, Is.False);
            Assert.That(result.ModifiedTimestampsIdentical, Is.False);
            Assert.That(result.LastModifiedTimestamp, Is.EqualTo(DateTimeOffset.MinValue));
        });
    }

    [Test]
    public void TestCompare_HashesAndTimestampsAreDifferent_ReturnsFullyDifferentResult()
    {
        // Arrange
        var current = new WebPageContent
        {
            Hash = "123",
            LastModified = DateTimeOffset.MaxValue
        };
        var previous = new WebPageContent
        {
            Hash = "456",
            LastModified = DateTimeOffset.MinValue
        };
        
        // Act
        var result = _comparer.Compare(current, previous);
        
        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.HashesIdentical, Is.False);
            Assert.That(result.ModifiedTimestampsIdentical, Is.False);
            Assert.That(result.LastModifiedTimestamp, Is.EqualTo(DateTimeOffset.MaxValue));
        });
    }

    [Test]
    public void TestCompare_TimestampsAreIdenticalHashesAreDifferent_ReturnsHashesDifferentResult()
    {
        // Arrange
        var current = new WebPageContent
        {
            Hash = "123",
            LastModified = DateTimeOffset.MinValue
        };
        var previous = new WebPageContent
        {
            Hash = "456",
            LastModified = DateTimeOffset.MinValue
        };

        // Act
        var result = _comparer.Compare(current, previous);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.HashesIdentical, Is.False);
            Assert.That(result.ModifiedTimestampsIdentical, Is.True);
            Assert.That(result.LastModifiedTimestamp, Is.EqualTo(DateTimeOffset.MinValue));
        });
    }

    [Test]
    public void TestCompare_AllIdentical_ReturnsIdenticalResult()
    {
        // Arrange
        var current = new WebPageContent
        {
            Hash = string.Empty,
            LastModified = DateTimeOffset.MinValue
        };
        var previous = new WebPageContent
        {
            Hash = string.Empty,
            LastModified = DateTimeOffset.MinValue
        };

        // Act
        var result = _comparer.Compare(current, previous);
        
        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.HashesIdentical, Is.True);
            Assert.That(result.ModifiedTimestampsIdentical, Is.True);
            Assert.That(result.LastModifiedTimestamp, Is.EqualTo(DateTimeOffset.MinValue));
        });
    }
}