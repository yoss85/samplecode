using System.Globalization;

namespace Dynamics365BC.Services;

public interface IEntitySyncTimeStampService
{
    Result<DateTime?> GetLastSyncTime<T>();
    Result SetSyncTime<T>();
}

public class EntitySyncTimeStampService: IEntitySyncTimeStampService 
{
    const string Format = "yyyy-MM-dd HH:mm:ss.fffffff";
    readonly IConnectorStorageService _connectorStorageService;
    readonly ITimeProvider _timeProvider;

    public EntitySyncTimeStampService(IConnectorStorageService storageService,
        ITimeProvider timeProvider)
    {
        _connectorStorageService = storageService;
        _timeProvider = timeProvider;
    }

    Result<DateTime?> IEntitySyncTimeStampService.GetLastSyncTime<T>()
    {
        var key = typeof(T).FullName;

        var result = _connectorStorageService.ReadString(key!);

        if (result.IsFailure)
            return Result.Failure<DateTime?>(result.Error);
        if (string.IsNullOrWhiteSpace(result.Value))
            return Result.Success(default(DateTime?));

        var dateTime = DateTime.ParseExact(result.Value, Format, CultureInfo.InvariantCulture);

        return Result.Success<DateTime?>(dateTime.ToLocalTime());
    }

    Result IEntitySyncTimeStampService.SetSyncTime<T>()
        => SetLastModifiedDateTimeInternal<T>(_timeProvider.UtcNow);

    Result SetLastModifiedDateTimeInternal<T>(DateTime lastModifiedDateTime)
    {
        var key = typeof(T).FullName;

        return _connectorStorageService.WriteString(key!, lastModifiedDateTime.ToUniversalTime().ToString(Format));
    }
}


