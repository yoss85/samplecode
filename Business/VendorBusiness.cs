using AvidConnect.DataModel;
using Dynamics365BC.Services;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace Dynamics365BC.Business;
public interface IVendorBusiness
{
    Task<Result<Vendor[]>> GetVendorsAsync();
}

public class VendorBusiness: BaseBusiness, IVendorBusiness
{
    readonly IDynamics365ApiClient _apiClient;
    readonly IDynamics365ConfigurationService _configuration;
    readonly IEntitySyncTimeStampService _entitySyncTimeStampService;
    readonly ILogger _logger;
    readonly IValidator<Vendor> _vendorValidator;

    public VendorBusiness(IDynamics365ApiClient apiClient,
        IDynamics365ConfigurationService configuration,
        IEntitySyncTimeStampService entitySyncTimeStampService,
        ILogger<IVendorBusiness> logger,
        IValidator<Vendor> vendorValidator) 
        : base(apiClient, 
            configuration,
            entitySyncTimeStampService, 
            logger)
    {
        _apiClient = apiClient;
        _configuration = configuration;
        _entitySyncTimeStampService = entitySyncTimeStampService;
        _logger = logger;
        _vendorValidator = vendorValidator;
    }

    async Task<Result<Vendor[]>> IVendorBusiness.GetVendorsAsync()
        => await GetForSingleCompanyInternalAsync(GetVendorsAsyncInternal, _configuration.SyncAllVendors);

    async Task<Result<Vendor[]>> GetVendorsAsyncInternal(
        Guid companyId,
        DateTime? lastModifiedDateTime)
    {
        var vendorsResult = await _apiClient.GetVendorsAsync(companyId, lastModifiedDateTime);

        if (vendorsResult.IsFailure)
            return Result.Failure<Vendor[]>(vendorsResult.Error);

        var mappedVendors = vendorsResult.Value
            .Select(Dynamics365Mapper.Map)
            .ToArray();

        // validate vendors gotten from D365
        var validationResults = mappedVendors
            .Select(_vendorValidator.Validate)
            .ToArray();

        if (validationResults.Any(r => !r.IsValid))
        {
            var errorMessage = AggregateValidationErrors(validationResults);

            _logger.LogError("One or more entities failed validation " + errorMessage);
            return Result.Failure<Vendor[]>(errorMessage);
        }

        // set the last successful sync
        _ = _entitySyncTimeStampService.SetSyncTime<Vendor>();

        return Result.Success(mappedVendors);
    }
}