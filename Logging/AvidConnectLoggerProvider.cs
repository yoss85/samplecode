using System.Collections.Concurrent;
using Microsoft.Extensions.Logging;

namespace Dynamics365BC.Logging;

[ExcludeFromCodeCoverage]
public sealed class AvidConnectLoggerProvider: ILoggerProvider
{
    readonly ConcurrentDictionary<string, AvidConnectLoggerWrapper> _loggers = new();
    readonly AvidConnect.Framework.Logging.ILogger _avidConnectLogger;

    public AvidConnectLoggerProvider(AvidConnect.Framework.Logging.ILogger avidConnectLogger)
    {
        _avidConnectLogger = avidConnectLogger;
    }

    public void Dispose()
    {
        _loggers.Clear();
    }

    public ILogger CreateLogger(string categoryName)
        => _loggers.GetOrAdd(categoryName, name => new AvidConnectLoggerWrapper(name, _avidConnectLogger));
}