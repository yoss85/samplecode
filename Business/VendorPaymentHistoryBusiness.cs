using AvidConnect.DataModel;
using Dynamics365BC.Services;
using Microsoft.Extensions.Logging;

namespace Dynamics365BC.Business;

public interface IVendorPaymentHistoryBusiness
{
    Task<Result<PaymentHistory[]>> GetVendorsPaymentsHistoryAsync();
}

public class VendorPaymentHistoryBusiness: BaseBusiness, IVendorPaymentHistoryBusiness
{
    readonly IDynamics365ApiClient _apiClient;
    readonly IDynamics365ConfigurationService _configuration;
    readonly ITimeProvider _timeProvider;

    public VendorPaymentHistoryBusiness(IDynamics365ApiClient apiClient,
        IDynamics365ConfigurationService configuration,
        IEntitySyncTimeStampService entitySyncTimeStampService,
        ILogger<IVendorPaymentHistoryBusiness> logger,
        ITimeProvider timeProvider) 
        : base(apiClient, configuration, entitySyncTimeStampService, logger)
    {
        _apiClient = apiClient;
        _configuration = configuration;
        _timeProvider = timeProvider;
    }

    async Task<Result<PaymentHistory[]>> IVendorPaymentHistoryBusiness.GetVendorsPaymentsHistoryAsync()
        => await GetForSingleCompanyInternalAsync(GetVendorPaymentsHistoryInternalAsync);


    async Task<Result<PaymentHistory[]>> GetVendorPaymentsHistoryInternalAsync(Guid companyId)
    {
        // sanity check so that if ui fails to send
        // the value in the appropriate range [1, 30
        var days = Math.Clamp(_configuration.NumberOfDaysToSyncPayments, 1, 30);
        var lastModifiedDateTime = _timeProvider.UtcNow.Subtract(TimeSpan.FromDays(days));

        var vendorPaymentJournalsResult = await _apiClient.GetAllVendorPaymentJournalsAsync(companyId, lastModifiedDateTime, true);

        if (vendorPaymentJournalsResult.IsFailure)
            return Result.Failure<PaymentHistory[]>(vendorPaymentJournalsResult.Error);

        List<PaymentHistory> paymentHistoryResult = new();
        foreach (var vendorPaymentJournal in vendorPaymentJournalsResult.Value)
        {
            var partialMappedVendorPayments = vendorPaymentJournal.VendorPayments?
                .Where(p => p.PostingDate.HasValue
                            && !string.IsNullOrWhiteSpace(p.DocumentNumber)
                            && p.Amount > 0)
                .ToArray();

            var mappedVendorPayments = partialMappedVendorPayments?
                .Select(Dynamics365Mapper.Map)
                .ToArray();

            if (mappedVendorPayments is null)
                continue;

            paymentHistoryResult.AddRange(mappedVendorPayments);
        }

        // search for payments that have invoices with no InvoiceNumber
        // InvoiceNumber comes from DynVendorPayment.ExternalDocumentNumber
        var invalidPayments = paymentHistoryResult?
            .Where(x => x.Invoices.Any(i => string.IsNullOrWhiteSpace(i.InvoiceNumber)))
            .ToList();

        if (invalidPayments?.Any() ?? false)
        {
            foreach (var payment in invalidPayments)
            {
                payment.ValidText =
                    $"Error - Required data point(s) not found. No invoice number for payment record with ExternalCode {payment.ExternalCode} and DocumentNumber {payment.PaymentNumber}";
            }
        }

        return Result.Success(paymentHistoryResult?.ToArray() ?? Array.Empty<PaymentHistory>());
    }
}

