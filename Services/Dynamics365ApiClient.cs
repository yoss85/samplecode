using System.Collections;
using System.Net.Http.Headers;
using Dynamics365BC.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Dynamics365BC.Services;

public interface IDynamics365ApiClient
{
    Task<Result<DynDimension[]>> GetDimensionsAsync(Guid companyId, bool expandDimensions, DateTime? lastModifiedDateTime);
    Task<Result<DynDimensionValue[]>> GetDimensionValuesAsync(Guid companyId, DateTime? lastModifiedDateTime);
    Task<Result<DynAccount[]>> GetAccountsAsync(Guid companyId, DateTime? lastModifiedDateTime);

    /// <summary>
    /// Get companies entities from dynamics 365
    /// </summary>
    /// <param name="companyName">Filter by company name</param>
    /// <param name="selectOnlyIds"></param>
    /// <returns></returns>
    Task<Result<DynCompany[]>> GetCompaniesAsync(bool selectOnlyIds = false, string? companyName = null);
    Task<Result<DynVendor[]>> GetVendorsAsync(Guid companyId, DateTime? lastModifiedDateTime);
    Task<Result<DynPurchaseInvoice>> CreatePurchaseInvoiceAsync(Guid companyId, DynPurchaseInvoice purchaseInvoice);
    Task<Result<DynPurchaseInvoiceLine>> CreatePurchaseInvoiceLineAsync(Guid companyId, Guid invoiceId, DynPurchaseInvoiceLine purchaseInvoiceLine);
    Task<Result<DynVendorPaymentJournal[]>> GetAllVendorPaymentJournalsAsync(Guid companyId, DateTime? lastModifiedDateTime, bool expandVendorPayments = false, string? journalDisplayName = null);
    Task<Result<DynPurchaseOrder[]>> GetPurchaseOrdersAsync(Guid companyId, bool expandPurchaseOrderLines = false);
    Task<Result<DynPurchaseReceipt[]>> GetPurchaseReceiptsAsync(Guid companyId, bool expandPurchaseReceiptLines = false);
    Task<Result<DynPurchaseInvoice[]>> GetPurchaseInvoicesAsync(Guid companyId, bool expandVendors = false);
    Task<Result<DynCompanyInformation>> GetCompanyInfoAsync(Guid companyId);
}

[ExcludeFromCodeCoverage]
public sealed class Dynamics365ApiClient : IDynamics365ApiClient
{
    const string ApiVersion = "v2.0";
    const string DateFormatString = "yyyy-MM-ddTHH:mm:ss.fffZ";
    readonly IHttpClientFactory _httpClientFactory;
    readonly IDynamics365ConfigurationService _configuration;
    readonly ILogger<Dynamics365ApiClient> _logger;
    readonly string _basePath;
    string _authorizationToken = string.Empty;

    public Dynamics365ApiClient(IHttpClientFactory httpClientFactory,
        IDynamics365ConfigurationService configuration,
        ILogger<Dynamics365ApiClient> logger)
    {
        _httpClientFactory = httpClientFactory;
        _configuration = configuration;
        _logger = logger;
        _basePath = new Uri($"/{configuration.TenantDomain}/api/{ApiVersion}", UriKind.Relative)
            .ToString();
    }

    #region IDynamics365ApiClient

    async Task<Result<DynDimension[]>> IDynamics365ApiClient.GetDimensionsAsync(
        Guid companyId,
        bool expandDimensionValues,
        DateTime? lastModifiedDateTime) 
        => await GetDimensionsInternalAsync(companyId, expandDimensionValues, lastModifiedDateTime);

    async Task<Result<DynDimension[]>> GetDimensionsInternalAsync(
        Guid companyId,
        bool expandDimensionValues,
        DateTime? lastModifiedDateTime)
        => await new DynamicsUriBuilder(_basePath)
            .Companies(companyId)
            .Bind(builder => builder.Dimensions()) 
            .BindIf(expandDimensionValues,
                builder => builder.Expand("dimensionValues"))
            .BindIf(lastModifiedDateTime.HasValue,
                builder => builder.Filter($"lastModifiedDateTime gt {lastModifiedDateTime!.Value.ToString(DateFormatString)}"))
            .Bind(builder => builder.Build())
            .Bind(GetAsync<DynDimension[]>);

    async Task<Result<DynDimensionValue[]>> IDynamics365ApiClient.GetDimensionValuesAsync(Guid companyId,
        DateTime? lastModifiedDateTime)
        => await new DynamicsUriBuilder(_basePath)
            .Companies(companyId)
            .Bind(builder => builder.DimensionValues())
            .BindIf(lastModifiedDateTime.HasValue,
                builder => builder.Filter($"lastModifiedDateTime gt {lastModifiedDateTime!.Value.ToString(DateFormatString)}"))
            .Bind(builder => builder.Build())
            .Bind(GetAsync<DynDimensionValue[]>);

    async Task<Result<DynAccount[]>> IDynamics365ApiClient.GetAccountsAsync(Guid companyId, DateTime? lastModifiedDateTime)
        => await GetAccountsInternalAsync(companyId, lastModifiedDateTime);

    async Task<Result<DynAccount[]>> GetAccountsInternalAsync(Guid companyId, DateTime? lastModifiedDateTime)
        => await new DynamicsUriBuilder(_basePath)
            .Companies(companyId)
            .Bind(builder => builder.Accounts())
            .BindIf(_ => lastModifiedDateTime.HasValue,
                builder => builder.Filter($"lastModifiedDateTime gt {lastModifiedDateTime!.Value.ToString(DateFormatString)}"))
            .Bind(builder => builder.Build())
            .Bind(GetAsync<DynAccount[]>);

    async Task<Result<DynCompany[]>> IDynamics365ApiClient.GetCompaniesAsync(bool selectOnlyIds, string? companyName)
        => await new DynamicsUriBuilder(_basePath)
            .Companies()
                .BindIf(companyName is not null, builder => builder.Filter($"name eq '{companyName}'"))
                .BindIf(selectOnlyIds, builder => builder.Select("id"))
            .Bind(builder => builder.Build())
            .Bind(GetAsync<DynCompany[]>);

    async Task<Result<DynVendor[]>> IDynamics365ApiClient.GetVendorsAsync(Guid companyId, DateTime? lastModifiedDateTime)
        => await GetVendorsInternalAsync(companyId, lastModifiedDateTime);

    async Task<Result<DynVendor[]>> GetVendorsInternalAsync(Guid companyId, DateTime? lastModifiedDateTime)
        => await new DynamicsUriBuilder(_basePath)
           .Companies(companyId)
           .Bind(builder => builder.Vendors())
           .BindIf(lastModifiedDateTime.HasValue,
                builder => builder.Filter($"lastModifiedDateTime gt {lastModifiedDateTime!.Value.ToUniversalTime().ToString(DateFormatString)}"))
           .Bind(builder => builder.Build())
           .Bind(GetAsync<DynVendor[]>);

    async Task<Result<DynPurchaseInvoice>> IDynamics365ApiClient.CreatePurchaseInvoiceAsync(Guid companyId, DynPurchaseInvoice purchaseInvoice)
        => await PostAsync($"{_basePath}/companies({companyId})/purchaseInvoices", purchaseInvoice);

    async Task<Result<DynPurchaseInvoiceLine>> IDynamics365ApiClient.CreatePurchaseInvoiceLineAsync(Guid companyId,
        Guid invoiceId,
        DynPurchaseInvoiceLine purchaseInvoiceLine)
        => await PostAsync($"{_basePath}/companies({companyId})/purchaseInvoices({invoiceId})/purchaseInvoiceLines", purchaseInvoiceLine);

    async Task<Result<DynVendorPaymentJournal[]>> IDynamics365ApiClient.GetAllVendorPaymentJournalsAsync(
        Guid companyId, DateTime? lastModifiedDateTime, bool expandVendorPayments, string? journalDisplayName)
        => await new DynamicsUriBuilder(_basePath)
            .Companies(companyId)
            .Bind(builder => builder.VendorPaymentJournals())
            .BindIf(expandVendorPayments, builder => builder.Expand("vendorPayments" + 
                   (string.IsNullOrWhiteSpace(journalDisplayName) ? "" : $"($filter = journalDisplayName eq '{journalDisplayName}')")))
            .BindIf(lastModifiedDateTime.HasValue,
                builder => builder.Filter($"lastModifiedDateTime gt {lastModifiedDateTime!.Value.ToUniversalTime().ToString(DateFormatString)}"))
            .Bind(builder => builder.Build())
            .Bind(GetAsync<DynVendorPaymentJournal[]>);

    async Task<Result<DynPurchaseOrder[]>> IDynamics365ApiClient.GetPurchaseOrdersAsync(Guid companyId, bool expandPurchaseOrderLines)
        => await GetAsync<DynPurchaseOrder[]>(
            expandPurchaseOrderLines
                ? $"{_basePath}/companies({companyId})/purchaseOrders?$expand=purchaseOrderLines"
                : $"{_basePath}/companies({companyId})/purchaseOrders");

    async Task<Result<DynPurchaseReceipt[]>> IDynamics365ApiClient.GetPurchaseReceiptsAsync(Guid companyId, bool expandPurchaseReceiptsLines)
        => await new DynamicsUriBuilder(_basePath)
           .Companies(companyId)
           .Bind(builder => builder.MaterialReceipts())
           .BindIf(expandPurchaseReceiptsLines,
                builder => builder.Expand("purchaseReceiptLines"))
           .Bind(builder => builder.Filter("orderNumber ne ''"))
           .Bind(builder => builder.Build())
           .Bind(GetAsync<DynPurchaseReceipt[]>);
    async Task<Result<DynPurchaseInvoice[]>> IDynamics365ApiClient.GetPurchaseInvoicesAsync(Guid companyId, bool expandVendors)
        => await new DynamicsUriBuilder(_basePath)
           .Companies(companyId)
           .Bind(builder => builder.PurchaseInvoices())
           .BindIf(expandVendors,
                builder => builder.Expand("vendor"))
           .Bind(builder => builder.Build())
           .Bind(GetAsync<DynPurchaseInvoice[]>);

    async Task<Result<DynCompanyInformation>> IDynamics365ApiClient.GetCompanyInfoAsync(Guid companyId)
        => await new DynamicsUriBuilder(_basePath)
        .Companies(companyId)
        .Bind(builder => builder.CompanyInformation())
        .Bind(builder => builder.Build())
        .Bind(GetAsync<DynCompanyInformation>);

    #endregion

    #region Private Methods

    async Task<Result<HttpRequestMessage>> CreateRequestAsync(string urlPath, HttpMethod httpMethod, object? content = null)
    {
        if (string.IsNullOrWhiteSpace(_authorizationToken))
        {
            _logger.LogInformation("Bearer Token not present.");
            var tokenResult = await GetBearerTokenInternal();
            if (tokenResult.IsFailure)
                return Result.Failure<HttpRequestMessage>("Authentication error: " + tokenResult.Error);

            if (string.IsNullOrWhiteSpace(tokenResult.Value.AccessToken))
                return Result.Failure<HttpRequestMessage>("Authentication error: Got empty Token");

            _authorizationToken = tokenResult.Value.AccessToken;
        }

        var request = new HttpRequestMessage();
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _authorizationToken);
        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        request.RequestUri = new Uri(urlPath, UriKind.Relative);
        request.Method = httpMethod;

        if (content is not null)
        {
            request.Headers.Add("ContentType", "application/json");
            request.Content = new StringContent(JsonConvert.SerializeObject(content, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
            }));
        }

        return request;
    }

    async Task<Result<T>> GetAsync<T>(string urlPath)
        => await CreateRequestAsync(urlPath, HttpMethod.Get)
            .Bind(SendAsync<T>);

    async Task<Result<T>> PostAsync<T>(string urlPath, T content)
        => await CreateRequestAsync(urlPath, HttpMethod.Post, content)
            .Bind(SendAsync<T>);

    async Task<Result<T>> SendAsync<T>(HttpRequestMessage request)
    {
        var httpClient = _httpClientFactory.CreateClient("Dynamics365BC");
        HttpResponseMessage response;
        try
        {
            response = await httpClient.SendAsync(request);
        }
        catch (HttpRequestException ex)
        {
            return Result.Failure<T>(ex.Message);
        }

        var content = await response.Content.ReadAsStringAsync();

        var deserializationErrors = new List<string>();
        // handle the case when response is an array of objects or an error
        // in that case the result will come in a property called "value"
        // so deserialize to "OdataResult<T>"
        if (typeof(IEnumerable).IsAssignableFrom(typeof(T)) || !response.IsSuccessStatusCode)
        {

            var data = JsonConvert.DeserializeObject<OdataResult<T>>(content, new JsonSerializerSettings
            {
                Error = (o, e) =>
                {
                    e.ErrorContext.Handled = true;
                    deserializationErrors.Add(e.ErrorContext.Error.Message);
                }
            });

            if (deserializationErrors.Count > 0)
            {
                var errorMessage = deserializationErrors.Aggregate((p, n) => $"{p}\n{n}");
                _logger.LogError("Error deserializing response: " + errorMessage);
                return Result.Failure<T>(errorMessage);
            }

            if (!response.IsSuccessStatusCode && data?.Error is null)
            {
                return Result.Failure<T>(response.ReasonPhrase);
            }

            if (!string.IsNullOrWhiteSpace(data?.Error?.Message))
            {
                var errorMessage = response.ReasonPhrase + ": " + data.Error.Message;
                _logger.LogError(errorMessage);
                return Result.Failure<T>(errorMessage);
            }

            return Result.Success(data!.Value)!;
        }

        var result = JsonConvert.DeserializeObject<T>(content, new JsonSerializerSettings
        {
            Error = (o, e) =>
            {
                e.ErrorContext.Handled = true;
                deserializationErrors.Add(e.ErrorContext.Error.Message);
            }
        });

        if (!response.IsSuccessStatusCode)
        {
            _logger.LogError(response.ReasonPhrase);
            return Result.Failure<T>(response.ReasonPhrase);
        }

        if (deserializationErrors.Count > 0)
        {
            var errorMessage = string.Concat(deserializationErrors, '\n');
            _logger.LogError("Error deserializing response: " + errorMessage);
            return Result.Failure<T>(errorMessage);
        }

        return Result.Success(result)!;
    }

    async Task<Result<TokenResponse>> GetBearerTokenInternal()
    {
        string clientId = _configuration.ClientId;
        string clientSecret = _configuration.ClientSecret;
        string tokenUrl = _configuration.ConfigurationTokenUrl;
        var httpClient = _httpClientFactory.CreateClient("Dynamics365BC");

        const string scope = "https://api.businesscentral.dynamics.com/.default";
        const string grantType = "client_credentials";

        var request = new HttpRequestMessage(HttpMethod.Post, tokenUrl);
        var formContentValues = new Dictionary<string, string>
            {
                { "grant_type", grantType },
                { "client_id", clientId },
                { "client_secret", clientSecret },
                { "scope", scope }
            };
        request.Content = new FormUrlEncodedContent(formContentValues);
        request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");

        _logger.LogInformation("");
        HttpResponseMessage response;
        try
        {
            response = await httpClient.SendAsync(request);
        }
        catch (Exception e)
        {
            _logger.LogCritical("Authentication attempt failed: " + e.Message);
            return Result.Failure<TokenResponse>(e.Message);
        }

        if (!response.IsSuccessStatusCode)
        {
            _logger.LogError("Authentication error: " + response.ReasonPhrase);
            return Result.Failure<TokenResponse>(response.ReasonPhrase);
        }

        var content = await response.Content.ReadAsStringAsync();

        List<string> deserializationErrors = new();
        var tokenResult = JsonConvert.DeserializeObject<TokenResponse>(content, new JsonSerializerSettings
        {
            Error = (o, e) =>
            {
                e.ErrorContext.Handled = true;
                deserializationErrors.Add(e.ErrorContext.Error.Message);
            }
        });

        if (deserializationErrors.Any() || tokenResult is null)
        {
            var errorMessage =
                $"Errors occurred deserializing auth token response: {string.Join(Environment.NewLine, deserializationErrors)}";

            _logger.LogError(errorMessage);

            Result.Failure<TokenResponse>(errorMessage);
        }

        _logger.LogInformation("Authentication succeeded");
        return Result.Success(tokenResult!);
    }

    #endregion
}

[ExcludeFromCodeCoverage]
public class OdataResult<T>
{
    [JsonProperty("@odata.context")]
    public string? Metadata { get; set; }

    [JsonProperty("odata.count")]
    public int Count { get; set; }

    public T? Value { get; set; }

    public ErrorResult? Error { get; set; }
}

[ExcludeFromCodeCoverage]
public class ErrorResult
{
    public string? Code { get; set; }
    public string? Message { get; set; }
}

[ExcludeFromCodeCoverage]
public class TokenResponse
{
    [JsonProperty("token_type")]
    public string? TokenType { get; set; }

    [JsonProperty("expires_in")]
    public int ExpiresIn { get; set; }

    [JsonProperty("ext_expires_in")]
    public int ExtExpiresIn { get; set; }

    [JsonProperty("access_token")]
    public string? AccessToken { get; set; }
}
