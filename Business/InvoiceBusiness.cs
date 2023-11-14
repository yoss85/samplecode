using AvidConnect.DataModel;
using Dynamics365BC.Models;
using Dynamics365BC.Services;
using Microsoft.Extensions.Logging;

namespace Dynamics365BC.Business;

public interface IInvoiceBusiness
{
    Task<Result<Invoice>> ExportInvoice(Invoice invoice);
}

public class InvoiceBusiness: BaseBusiness, IInvoiceBusiness
{
    readonly IDynamics365ApiClient _apiClient;
    readonly IDynamics365ConfigurationService _configuration;
    readonly ILogger _logger;

    public InvoiceBusiness(IDynamics365ApiClient apiClient,
        IDynamics365ConfigurationService configuration,
        IEntitySyncTimeStampService entitySyncTimeStampService,
        ILogger<IInvoiceBusiness> logger) 
        : base(apiClient, configuration, entitySyncTimeStampService, logger)
    {
        _apiClient = apiClient;
        _configuration = configuration;
        _logger = logger;
    }

    async Task<Result<Invoice>> IInvoiceBusiness.ExportInvoice(Invoice invoice)
    {
        string currencyCodeGroupName = _configuration.CurrencyCodeGroupName;
        string companyName = _configuration.CompanyName;

        (DynPurchaseInvoice Invoice, DynPurchaseInvoiceLine[] InvoiceLines) invoiceAndLines = default!;

        var companyResult = await _apiClient.GetCompanyIdByNameAsync(companyName);
        if (companyResult.IsFailure)
            return Result.Failure<Invoice>(companyResult.Error);

        if (companyResult.Value is null)
        {
            var errorMessage = $"Did not found any company with the name '{_configuration.CompanyName}'";
            _logger.LogError(errorMessage);
            return Result.Failure<Invoice>(errorMessage);
        }

        var companyId = companyResult.Value.Id;
        var createPurchaseInvoiceResult = await Dynamics365Mapper.Map(invoice, currencyCodeGroupName)
            .Tap(invAndLines => invoiceAndLines = invAndLines)
            .Bind(async _ => await _apiClient.CreatePurchaseInvoiceAsync(companyId, invoiceAndLines.Invoice!))
            .Bind(async pi =>
            {
                foreach (var line in invoiceAndLines.InvoiceLines!)
                {
                    var result = await _apiClient.CreatePurchaseInvoiceLineAsync(
                        companyId,
                        pi.Id!.Value,
                        line
                        );

                    if (result.IsFailure)
                        return Result.Failure(result.Error);
                }

                return Result.Success();
            });

        return createPurchaseInvoiceResult.IsFailure
            ? Result.Failure<Invoice>(createPurchaseInvoiceResult.Error)
            : Result.Success(invoice);
    }
}

