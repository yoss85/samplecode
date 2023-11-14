using AvidConnect.DataModel;
using Connector.Shared.Contracts;

namespace Dynamics365BC;

[ExcludeFromCodeCoverage]
public class Dynamics365BCSourceConfig : IConnectorV2Config
{
    public int ConnectorId => 1;
    public ConnectorType ConnectorType => ConnectorType.SystemConnector;
    public SourceType SourceType => SourceType.AccountingSystem;
    public bool IsDiffed => true;
    public bool IsScheduled => true;
    public Task StartupAsync(CancellationTokenSource cts) => Task.CompletedTask;
}

[ExcludeFromCodeCoverage]
public class Dynamics365BCTargetConfig : IConnectorV2Config
{
    public int ConnectorId => 1;
    public ConnectorType ConnectorType => ConnectorType.SystemConnector;
    public SourceType SourceType => SourceType.Platform;
    public bool IsDiffed => false;
    public bool IsScheduled => false;
    public Task StartupAsync(CancellationTokenSource cts) => Task.CompletedTask;
}