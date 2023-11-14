using AvidConnect.DataModel;
using Dynamics365BC.Services;
using Microsoft.Extensions.Logging;

namespace Dynamics365BC.Business;


public interface IMaterialReceiptBusiness
{
    Task<Result<MaterialReceipt[]>> GetMaterialReceiptsAsync();
}

public class MaterialReceiptBusiness: BaseBusiness, IMaterialReceiptBusiness
{
    readonly IDynamics365ApiClient _apiClient;

    public MaterialReceiptBusiness(IDynamics365ApiClient apiClient,
        IDynamics365ConfigurationService configuration,
        IEntitySyncTimeStampService entitySyncTimeStampService,
        ILogger<IMaterialReceiptBusiness> logger)
        : base(apiClient, configuration, entitySyncTimeStampService, logger)
    {
        _apiClient = apiClient;
    }

    async Task<Result<MaterialReceipt[]>> IMaterialReceiptBusiness.GetMaterialReceiptsAsync()
        => await GetForSingleCompanyInternalAsync(GetMaterialReceiptsInternalAsync);

    async Task<Result<MaterialReceipt[]>> GetMaterialReceiptsInternalAsync(Guid companyId)
    {
        List<MaterialReceipt> materialReceipts = new();

        var materialReceiptsResult = await _apiClient.GetPurchaseReceiptsAsync(companyId, true);

        if (materialReceiptsResult.IsFailure)
            return Result.Failure<MaterialReceipt[]>(materialReceiptsResult.Error);

        var mappedMaterialReceipts = materialReceiptsResult.Map(dynMRs => dynMRs.Select(Dynamics365Mapper.Map).ToArray());

        foreach (var purchaseReceipt in mappedMaterialReceipts.Value)
        {
            materialReceipts.AddRange(purchaseReceipt);
        }

        return materialReceipts.ToArray();
    }
}

