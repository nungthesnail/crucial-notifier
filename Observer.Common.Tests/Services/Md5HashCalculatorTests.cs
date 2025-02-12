using System.Text;
using Observer.Common.Implementations.Services;

namespace Observer.Common.Tests.Services;

public sealed class Md5HashCalculatorTests
{
    private readonly Md5HashCalculator _calculator = new();
    
    [TestCase("1234", "81dc9bdb52d04dc20036dbd8313ed055")]
    [TestCase("5678", "674f3c2c1a8a6f90461e8a66fb5550ba")]
    [TestCase("", "d41d8cd98f00b204e9800998ecf8427e")]
    [TestCase("abcd0987", "614ed8a855d08f348405b24e3c376080")]
    public async Task TestCalculateHashAsync_InputIsString_ResultIsCorrectHash(string input, string expectedHash)
    {
        // Arrange
        var inputStream = CreateStreamFromString(input);
        
        // Act
        var hash = await _calculator.CalculateHashAsync(inputStream);
        
        // Assert
        Assert.That(hash, Is.EqualTo(expectedHash));
    }

    [Test]
    public void TestCalculateHashAsync_InputIsNull_ThrowsArgumentNullException()
    {
        // Arrange
        Stream inputStream = null!;
        
        // Act
        var task = _calculator.CalculateHashAsync(inputStream);
        
        // Assert
        Assert.ThrowsAsync<ArgumentNullException>(async () => await task);
    }
    
    private static MemoryStream CreateStreamFromString(string s)
        => new(Encoding.UTF8.GetBytes(s));
}