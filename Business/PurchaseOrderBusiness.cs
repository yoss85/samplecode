using AvidConnect.DataModel;
using Dynamics365BC.Services;
using Microsoft.Extensions.Logging;

namespace Dynamics365BC.Business;

public interface IPurchaseOrderBusiness
{
    Task<Result<PurchaseOrder[]>> GetPurchaseOrdersAsync();
}

public class PurchaseOrderBusiness : BaseBusiness, IPurchaseOrderBusiness
{
    readonly IDynamics365ApiClient _apiClient;

    public PurchaseOrderBusiness(IDynamics365ApiClient apiClient,
        IDynamics365ConfigurationService configuration,
        IEntitySyncTimeStampService entitySyncTimeStampService,
        ILogger<IPurchaseOrderBusiness> logger)
        : base(apiClient, configuration, entitySyncTimeStampService, logger)
    {
        _apiClient = apiClient;
    }

    async Task<Result<PurchaseOrder[]>> IPurchaseOrderBusiness.GetPurchaseOrdersAsync()
        => await GetForSingleCompanyInternalAsync(GetPurchaseOrdersInternalAsync);

    async Task<Result<PurchaseOrder[]>> GetPurchaseOrdersInternalAsync(Guid companyId)
        => await _apiClient.GetPurchaseOrdersAsync(companyId, true)
            .Map(dynPOs => dynPOs?.Select(Dynamics365Mapper.Map).ToArray() ?? Array.Empty<PurchaseOrder>());
}

