using Dynamics365BC.Extensions;
using Dynamics365BC.Services;
using FluentValidation.Results;
using Microsoft.Extensions.Logging;

namespace Dynamics365BC.Business;

public class BaseBusiness
{
    readonly IDynamics365ApiClient _apiClient;
    readonly IDynamics365ConfigurationService _configuration;
    readonly IEntitySyncTimeStampService _entitySyncTimeStampService;
    readonly ILogger _logger;

    public BaseBusiness(IDynamics365ApiClient apiClient, 
        IDynamics365ConfigurationService configuration,
        IEntitySyncTimeStampService entitySyncTimeStampService,
        ILogger logger
        )
    {
        _apiClient = apiClient;
        _configuration = configuration;
        _entitySyncTimeStampService = entitySyncTimeStampService;
        _logger = logger;
    }

    async Task<Result<Guid>> GetCompanyId()
    { 
        var companyResult = await _apiClient.GetCompanyIdByNameAsync(_configuration.CompanyName);

        if (companyResult.IsFailure)
            return Result.Failure<Guid>(companyResult.Error);

        if (companyResult.Value is null)
        {
            var errorMessage = $"Did not found any company with the name '{_configuration.CompanyName}'";
            _logger.LogError(errorMessage);
            return Result.Failure<Guid>(errorMessage);
        }

        return companyResult.Value.Id;
    }

    protected async Task<Result<T[]>> GetForSingleCompanyInternalAsync<T>(
        Func<Guid, DateTime?, Task<Result<T[]>>> integrationFunction, bool syncAll)
    {
        DateTime? lastModifiedDateTime = null!;
        if (!syncAll)
        {
            var lastModifiedDateTimeResult = _entitySyncTimeStampService.GetLastSyncTime<T>();
            if (lastModifiedDateTimeResult.IsFailure)
                return Result.Failure<T[]>(lastModifiedDateTimeResult.Error);

            lastModifiedDateTime = lastModifiedDateTimeResult.Value;
        }

        var operationResult = await GetCompanyId()
            .Bind(id => integrationFunction(id, lastModifiedDateTime));

        return operationResult;
    }

    protected async Task<Result<T[]>> GetForSingleCompanyInternalAsync<T>(
        Func<Guid, Task<Result<T[]>>> integrationFunc)
        => await GetCompanyId().Bind(id => integrationFunc(id));


        protected static string AggregateValidationErrors(IEnumerable<ValidationResult> validationResults)
        => validationResults
            .Where(r => r.IsValid == false)
            .SelectMany(r => r.Errors)
            .Select(x => x.ErrorMessage)
            .Aggregate((p, n) => $"{p}\n{n}");
}