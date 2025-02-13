using Microsoft.Extensions.Logging;

namespace Tests.Common.Implementations;

public class LoggerStub<T> : ILogger<T>
{
    public IDisposable? BeginScope<TState>(TState state) where TState : notnull => null;
    public bool IsEnabled(LogLevel logLevel) => false;
    public void Log<TState>(LogLevel _1, EventId _2, TState _3, Exception? _4,
        Func<TState, Exception?, string> _5)
    { }
}
