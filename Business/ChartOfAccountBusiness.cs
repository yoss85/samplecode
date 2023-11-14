using AvidConnect.DataModel;
using Dynamics365BC.Services;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace Dynamics365BC.Business;

public interface IChartOfAccountBusiness
{
    Task<Result<EnterpriseCode[]>> GetChartOfAccountsAsync();
}

public class ChartOfAccountBusiness: BaseBusiness, IChartOfAccountBusiness
{
    readonly IDynamics365ApiClient _apiClient;
    readonly IDynamics365ConfigurationService _configuration;
    readonly IEntitySyncTimeStampService _entitySyncTimeStampService;
    readonly ILogger _logger;
    readonly IValidator<EnterpriseCode> _enterpriseCodeValidator;

    public ChartOfAccountBusiness(IDynamics365ApiClient apiClient,
        IDynamics365ConfigurationService configuration,
        IEntitySyncTimeStampService entitySyncTimeStampService,
        ILogger<ChartOfAccountBusiness> logger,
        IValidator<EnterpriseCode> enterpriseCodeValidator) 
        : base(apiClient, configuration, entitySyncTimeStampService, logger)
    {
        _apiClient = apiClient;
        _configuration = configuration;
        _entitySyncTimeStampService = entitySyncTimeStampService;
        _logger = logger;
        _enterpriseCodeValidator = enterpriseCodeValidator;
    }
    async Task<Result<EnterpriseCode[]>> IChartOfAccountBusiness.GetChartOfAccountsAsync()
        => await GetForSingleCompanyInternalAsync(GetChartOfAccountsInternalAsync, _configuration.SyncAllCodes);

    async Task<Result<EnterpriseCode[]>> GetChartOfAccountsInternalAsync(
        Guid companyId,
        DateTime? lastModifiedDateTime
        )
    {
        List<EnterpriseCode> enterpriseCodes = new();

        var operationResult = await _apiClient.GetAccountsAsync(companyId, lastModifiedDateTime)
            .Map(accounts => accounts.Select(Dynamics365Mapper.Map))
            .Tap(enterpriseCodes.AddRange)
            .Bind(async _ => await _apiClient.GetDimensionsAsync(companyId, true, lastModifiedDateTime))
            .Map(dimensions => dimensions.Select(Dynamics365Mapper.Map).SelectMany(x => x))
            .Tap(enterpriseCodes.AddRange)
            .Bind(_ =>
            {
                var validationResults = enterpriseCodes
                    .Select(enterpriseCode => _enterpriseCodeValidator.Validate(enterpriseCode))
                    .ToArray();

                if (validationResults.Any(r => !r.IsValid))
                {
                    var validationErrors = AggregateValidationErrors(validationResults);
                    var message = "One or more entities failed validation " + validationErrors;
                    _logger.LogError(message);
                    return Result.Failure(validationErrors);
                }

                return Result.Success();
            })
            .Bind(() => _entitySyncTimeStampService.SetSyncTime<EnterpriseCode>());

        return operationResult.IsFailure
            ? Result.Failure<EnterpriseCode[]>(operationResult.Error)
            : Result.Success(enterpriseCodes.ToArray());
    }
}

