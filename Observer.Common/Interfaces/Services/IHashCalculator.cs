namespace Observer.Common.Interfaces.Services;

public interface IHashCalculator
{
    Task<string> CalculateHashAsync(Stream content);
}