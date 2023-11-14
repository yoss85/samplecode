using AvidConnect.Framework;
using AvidConnect.Framework.Serialization;

namespace Dynamics365BC.Services;

public interface IConnectorStorageService
{
    public Result<string> ReadString(string key);

    public Result WriteString(string key, string value);
}

[ExcludeFromCodeCoverage]
public class ConnectorStorageService : IConnectorStorageService
{
    readonly object _connectorLock = new();
    readonly IConnector _connector;

    public ConnectorStorageService(IConnector connector)
    {
        _connector = connector;
    }

    public Result<string> ReadString(string key)
    {
        if (string.IsNullOrWhiteSpace(key))
            return Result.Failure<string>("Argument 'key' must be a valid string");

        string result;
        try
        {
            lock (_connectorLock)
            {
                result = _connector.CacheRead<string>(key);
            }
        }
        catch (Exception e)
        {
            return Result.Failure<string>(e.Message);
        }

        return result;
    }

    public Result WriteString(string key, string value)
    {
        if (string.IsNullOrWhiteSpace(key))
            return Result.Failure<string>("Argument 'key' must be a valid string");

        try
        {
            lock (_connectorLock)
            {
                _connector.CacheWrite(key, value);
            }
        }
        catch (Exception e)
        {
            return Result.Failure<string>(e.Message);
        }

        return Result.Success();
    }
}