using Connector.Shared.Contracts;
using Microsoft.Extensions.DependencyInjection;

namespace Dynamics365BC;

[ExcludeFromCodeCoverage]
public class Dynamics365BCService : IConnectorServiceV2
{
    public Dynamics365BCService() { }

    public static void AddDependencyInjections(IServiceCollection services, IConnectorServiceConfig config)
    {
    }

    public Task StartupAsync(CancellationTokenSource cts) => Task.CompletedTask;

    public void Dispose() { }
}