namespace Dynamics365BC;

public abstract class ODataUriBuilderBase
{
    readonly string _baseUri;
    string _entity = null!;
    protected List<string> Filters { get; init; } = new();
    protected List<string> Selects { get; init; }= new();
    protected List<string> OrderBys { get; init; }= new();
    protected List<string> Expands { get; init; }= new();
    protected int? _tops;
    protected int? _skips;

    protected ODataUriBuilderBase(string baseUri)
    {
        _baseUri = baseUri;
    }

    protected ODataUriBuilderBase ForEntity(string entity, Guid? id = null)
    {
        _entity = string.IsNullOrWhiteSpace(_entity)
            ? entity
            : _entity + entity;

        if (id.HasValue)
            _entity += $"({id})";
        return this;
    }

    public Result<string> Build()
    {
        if (string.IsNullOrWhiteSpace(_entity))
            Result.Success("Entity is not specified");

        var queryParts = new List<string>();

        if (Filters.Any())
            queryParts.Add("$filter=" + string.Join(" and", Filters));

        if (Selects.Any())
            queryParts.Add("$select=" + string.Join(",", Selects));

        if (Expands.Any())
            queryParts.Add("$expand=" + string.Join(",", Expands));

        if (OrderBys.Any())
            queryParts.Add("$orderBy=" + string.Join(",", OrderBys));

        if (_tops.HasValue)
            queryParts.Add("$top=" + _tops.Value);

        if (_skips.HasValue)
            queryParts.Add("$skip=" + _skips.Value);

        var path = $"{_baseUri}/{_entity}";
        if (queryParts.Any())
        {
            path += $"?{string.Join("&", queryParts)}";
        }

        return Result.Success(path);
    }
}

public class DynamicsUriBuilder: ODataUriBuilderBase
{
    public DynamicsUriBuilder(string baseUri) : base(baseUri)
    {
    }

    public Result<DynamicsUriBuilder> Companies(Guid? companyId = null)
        => CreateEntitySegment("companies", companyId);

    public Result<DynamicsUriBuilder> Dimensions(Guid? dimensionsId = null) 
        => CreateEntitySegment("/dimensions", dimensionsId);

    public Result<DynamicsUriBuilder> DimensionValues(Guid? dimensionValueId = null)
        => CreateEntitySegment("/dimensionValues",  dimensionValueId);

    public Result<DynamicsUriBuilder> Accounts(Guid? accountId = null)
        => CreateEntitySegment("/accounts", accountId);

    public Result<DynamicsUriBuilder> Vendors(Guid? vendorId = null)
        => CreateEntitySegment("/vendors", vendorId);

    public Result<DynamicsUriBuilder> MaterialReceipts(Guid? purchaseReceiptId = null)
        => CreateEntitySegment("/purchaseReceipts", purchaseReceiptId);

    public Result<DynamicsUriBuilder> VendorPaymentJournals(Guid? vendorPaymentJournalId = null)
        => CreateEntitySegment("/vendorPaymentJournals", vendorPaymentJournalId);

    public Result<DynamicsUriBuilder> PurchaseInvoices(Guid? purchaseInvoiceId = null)
    => CreateEntitySegment("/purchaseInvoices", purchaseInvoiceId);

    public Result<DynamicsUriBuilder> CompanyInformation()
        => CreateEntitySegment("/companyInformation", null);

    public Result<DynamicsUriBuilder> Filter(string filter)
    {
        if (!string.IsNullOrWhiteSpace(filter)) 
            Filters.Add(filter);

        return this;
    }

    public Result<DynamicsUriBuilder> Select(params string[] fields)
    {
        if (fields.Length > 0)
            Selects.AddRange(fields);

        return this;
    }

    public Result<DynamicsUriBuilder> Expand(params string[] relationships)
    {
        if (relationships.Length > 0 )
            Expands.AddRange(relationships);

        return this;
    }

    public Result<DynamicsUriBuilder> OrderBy(string orderByField, bool descending = false)
    {
        if (!string.IsNullOrWhiteSpace(orderByField))
            OrderBys.Add(orderByField + (descending ? " desc" : string.Empty));

        return this;
    }

    public Result<DynamicsUriBuilder> Top(int top)
    {
        _tops = top;
        return this;
    }

    public Result<DynamicsUriBuilder> Skip(int skip)
    {
        _skips = skip;
        return this;
    }

    DynamicsUriBuilder CreateEntitySegment(string entityName, Guid? entityId)
    {
        var partialUri = entityName;

        if (entityId.HasValue)
            partialUri += $"({entityId})";

        ForEntity(partialUri);
        return this;
    }
}
