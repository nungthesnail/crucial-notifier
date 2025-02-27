namespace Observer.Common.Interfaces.Services;

public interface IHashCalculator
{
    Task<string> CalculateHashAsync(string content);
}