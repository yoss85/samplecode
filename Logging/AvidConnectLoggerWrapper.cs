using Microsoft.Extensions.Logging;

namespace Dynamics365BC.Logging;

[ExcludeFromCodeCoverage]
public sealed class AvidConnectLoggerWrapper: ILogger
{
    readonly string _name;
    readonly AvidConnect.Framework.Logging.ILogger _avidConnectLogger;

    public AvidConnectLoggerWrapper(string name, AvidConnect.Framework.Logging.ILogger avidConnectLogger)
    {
        _name = name;
        _avidConnectLogger = avidConnectLogger;
    }

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        string message = _name + ": " + state;

        switch (logLevel)
        {
            case LogLevel.Trace:
                _avidConnectLogger.Debug(message);
                return;
            case LogLevel.Debug:
                _avidConnectLogger.Debug(message);
                return;
            case LogLevel.Information:
                _avidConnectLogger.Info(message);
                return;
            case LogLevel.Warning:
                _avidConnectLogger.Warn(message);
                return;
            case LogLevel.Error:
                _avidConnectLogger.Error(message);
                return;
            case LogLevel.Critical:
                _avidConnectLogger.Fatal(message);
                return;
            case LogLevel.None:
                return;
        }
    }

    public bool IsEnabled(LogLevel logLevel) => true;

    public IDisposable? BeginScope<TState>(TState state) where TState : notnull
        => default!;

}
