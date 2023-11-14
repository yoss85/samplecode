namespace Dynamics365BC.Services;


public interface ITimeProvider
{
    DateTime UtcNow { get; }
}

public class TimeProvider: ITimeProvider
{
    public DateTime UtcNow => DateTime.UtcNow;
}
