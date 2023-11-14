using AvidConnect.DataModel;
using AvidConnect.Framework;
using Dynamics365BC.Business;
using Dynamics365BC.Services;
using Dynamics365BC.Validators;
using FluentValidation;
using Dynamics365BC.Logging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Contrib.WaitAndRetry;
using Polly.Extensions.Http;

namespace Dynamics365BC.Extensions;

[ExcludeFromCodeCoverage]
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDynamics365Services(this IServiceCollection services,
        IConnector? connector,
        string baseAddress,
        IAsyncPolicy<HttpResponseMessage>? dynamics365HttpClientPolicy = null
    )
    {
        services.AddSingleton(connector);

        if (connector?.Logger is not null)
        {
            services.AddSingleton(connector.Logger);
            services.AddAvidConnectLogger();
        }

        services.AddScoped<IConnectorStorageService, ConnectorStorageService>();

        const int defaultMaxRetryCount = 3;
        var policy = dynamics365HttpClientPolicy ?? GetDefaultPolicy(defaultMaxRetryCount);

        services.AddHttpClient("Dynamics365BC", client =>
        {
            client.BaseAddress = new Uri(baseAddress);
        }).AddPolicyHandler(policy);

        services.AddScoped<IDynamics365ApiClient, Dynamics365ApiClient>();
        services.AddScoped<IChartOfAccountBusiness, ChartOfAccountBusiness>();
        services.AddScoped<IInvoiceBusiness, InvoiceBusiness>();
        services.AddScoped<IMaterialReceiptBusiness, MaterialReceiptBusiness>();
        services.AddScoped<IPurchaseOrderBusiness, PurchaseOrderBusiness>();
        services.AddScoped<IVendorBusiness, VendorBusiness>();
        services.AddScoped<IVendorPaymentHistoryBusiness, VendorPaymentHistoryBusiness>();
        services.AddScoped<IPaymentRequestBusiness, PaymentRequestBusiness>();
        services.AddScoped<IEntitySyncTimeStampService, EntitySyncTimeStampService>();
        services.AddScoped<ITimeProvider, TimeProvider>();
        services.AddScoped<IDynamics365ConfigurationService, Dynamics365ConfigurationService>();

        RegisterValidators(services);

        return services;
    }

    static void RegisterValidators(IServiceCollection services)
    {
        services.AddSingleton<IValidator<Vendor>, VendorValidator>();
        services.AddSingleton<IValidator<EnterpriseCode>, EnterpriseCodeValidator>();
    }

    public static void AddAvidConnectLogger(this IServiceCollection services)
        => services.TryAddEnumerable(ServiceDescriptor.Singleton<ILoggerProvider, AvidConnectLoggerProvider>());

    static IAsyncPolicy<HttpResponseMessage> GetDefaultPolicy(int retryCount)
        => HttpPolicyExtensions.HandleTransientHttpError()
            .WaitAndRetryAsync(
                Backoff.DecorrelatedJitterBackoffV2(
                    medianFirstRetryDelay: TimeSpan.FromSeconds(1),
                    retryCount: retryCount));
}
