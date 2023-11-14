using AvidConnect.Framework;

namespace Dynamics365BC.Services;

public interface IDynamics365ConfigurationService
{
    string CompanyName { get;}
    string ClientSecret { get; }
    string ClientId { get; }
    string ConfigurationTokenUrl { get; }
    string TenantDomain { get; }
    Uri DynamicsBaseUrl { get; }
    string CurrencyCodeGroupName { get; }
    bool SyncAllCodes { get; }
    bool SyncAllVendors { get; }
    int NumberOfDaysToSyncPayments { get; }
}

[ExcludeFromCodeCoverage]
public sealed class Dynamics365ConfigurationService : IDynamics365ConfigurationService
{
    readonly IConnector _connector;

    public string CompanyName => _connector.Config.GetValue(Dynamics365ConfigurationGroupNames.ConnectionGroupName,
        Dynamics365ConfigurationKeys.CompanyName) ?? throw new UserFriendlyException("Connector configuration error: A company name must be specified in configuration.");

    public string ClientSecret => _connector.Config.GetValue(Dynamics365ConfigurationGroupNames.ConnectionGroupName,
        Dynamics365ConfigurationKeys.ClientSecret);

    public string ClientId => _connector.Config.GetValue(Dynamics365ConfigurationGroupNames.ConnectionGroupName,
        Dynamics365ConfigurationKeys.ClientId);

    public string ConfigurationTokenUrl => _connector.Config.GetValue(
        Dynamics365ConfigurationGroupNames.ConnectionGroupName,
        Dynamics365ConfigurationKeys.TokenUrl);

    public string TenantDomain => _connector.Config.GetValue(Dynamics365ConfigurationGroupNames.ConnectionGroupName,
        Dynamics365ConfigurationKeys.TenantDomain);

    public Uri DynamicsBaseUrl => new(_connector.Config.GetValue(
        Dynamics365ConfigurationGroupNames.ConnectionGroupName,
        Dynamics365ConfigurationKeys.DynamicsBaseUrl));

    public string CurrencyCodeGroupName => _connector.Config.GetValue(
        Dynamics365ConfigurationGroupNames.CurrencyCodeGroupName,
        Dynamics365ConfigurationKeys.CurrencyCode);

    public bool SyncAllCodes =>
        _connector.Config.GetObject<CodeSyncSettings>(Dynamics365ConfigurationGroupNames.CodeSync)?.SyncAll ?? false;

    public bool SyncAllVendors =>
        _connector.Config.GetObject<VendorSyncSettings>(Dynamics365ConfigurationGroupNames.VendorSync)?.SyncAll ?? false;

    public int NumberOfDaysToSyncPayments =>
        _connector.Config
        .GetObject<PaymentHistorySyncSettings>(Dynamics365ConfigurationGroupNames.PaymentHistorySyncSettings)
            .NumberOfDaysToSyncPayments;

    public Dynamics365ConfigurationService(IConnector connector)
    {
        _connector = connector;
    }
}

[ExcludeFromCodeCoverage]
public sealed class Dynamics365ConfigurationKeys
{
    public const string TokenUrl = "TokenUrl";
    public const string ClientId = "ClientId";
    public const string ClientSecret = "ClientSecret";
    public const string TenantDomain = "TenantDomain";
    public const string DynamicsBaseUrl = "DynamicsBaseUrl";
    public const string CurrencyCode = "CurrencyCode";
    public const string SyncAll = "SyncAll";
    public const string CompanyName = "CompanyName";
    public const string NumberOfDaysToSyncPayments = "NumberOfDaysToSyncPayments";
}

[ExcludeFromCodeCoverage]
public sealed class Dynamics365ConfigurationGroupNames
{
    public const string ConnectionGroupName = "Connection";
    public const string CurrencyCodeGroupName = "CurrencyCodeGroupName";
    public const string CodeSync = "CodeSync";
    public const string VendorSync = "VendorSync";
    public const string PaymentHistorySyncSettings = "PaymentHistorySyncSettings";
}

[ExcludeFromCodeCoverage]
public class CodeSyncSettings
{
    public bool SyncAll { get; set; } = true;
}

[ExcludeFromCodeCoverage]
public class VendorSyncSettings
{
    public bool SyncAll { get; set; } = true;
}

[ExcludeFromCodeCoverage]
public class PaymentHistorySyncSettings
{
    public int NumberOfDaysToSyncPayments { get; set; }
}
