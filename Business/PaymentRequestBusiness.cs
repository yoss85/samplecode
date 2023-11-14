using AvidConnect.DataModel;
using Dynamics365BC.Models;
using Dynamics365BC.Services;
using Microsoft.Extensions.Logging;

namespace Dynamics365BC.Business;

public interface IPaymentRequestBusiness
{
    Task<Result<PaymentRequest[]>> GetPaymentRequestsAsync();
}

public class PaymentRequestBusiness: BaseBusiness, IPaymentRequestBusiness
{
    readonly IDynamics365ApiClient _apiClient;
    readonly ITimeProvider _timeProvider;

    public PaymentRequestBusiness(
        IDynamics365ApiClient apiClient,
        IDynamics365ConfigurationService configuration,
        IEntitySyncTimeStampService entitySyncTimeStampService,
        ITimeProvider timeProvider,
        ILogger<PaymentRequestBusiness> logger) 
        : base(apiClient, configuration, entitySyncTimeStampService, logger)
    {
        _apiClient = apiClient;
        _timeProvider = timeProvider;
    }

    async Task<Result<PaymentRequest[]>> IPaymentRequestBusiness.GetPaymentRequestsAsync()
        => await GetForSingleCompanyInternalAsync(GetPaymentRequestsInternalAsync);

    async Task<Result<PaymentRequest[]>> GetPaymentRequestsInternalAsync(Guid companyId)
    {
        List<PaymentRequest> paymentRequests = new();

        var companyInfoTask = _apiClient.GetCompanyInfoAsync(companyId);
        var vendorPaymentJournalsTask = _apiClient.GetAllVendorPaymentJournalsAsync(companyId, null, true, "AVIDX");
        var purchaseInvoicesTask = _apiClient.GetPurchaseInvoicesAsync(companyId, true);

        try
        {
            await Task.WhenAll(companyInfoTask, vendorPaymentJournalsTask, purchaseInvoicesTask);
        }
        catch (AggregateException ae)
        {
            string message = string.Empty;
            foreach (var innerEx in ae.InnerExceptions)
            {
                message += innerEx.Message + Environment.NewLine;
            }

            return Result.Failure<PaymentRequest[]>(message);
        }

        var purchaseInvoicesResult = purchaseInvoicesTask.Result;
        var vendorPaymentJournalsResult = vendorPaymentJournalsTask.Result;
        var companyInfoResult = companyInfoTask.Result;

        if (companyInfoResult.IsFailure)
            return Result.Failure<PaymentRequest[]>(companyInfoTask.Result.Error);
        
        if (vendorPaymentJournalsResult.IsFailure)
            return Result.Failure<PaymentRequest[]>(vendorPaymentJournalsTask.Result.Error);

        if (purchaseInvoicesResult.IsFailure)
            return Result.Failure<PaymentRequest[]>(purchaseInvoicesResult.Error);

        var purchaseInvoices = purchaseInvoicesResult.Value;
        var vendorPaymentJournals = vendorPaymentJournalsResult.Value;
        var companyInfo = companyInfoResult.Value;

        if (vendorPaymentJournals == null
            || companyInfo == null 
            || purchaseInvoices == null)
        {
            return paymentRequests.ToArray();
        }

        foreach (var vendorPaymentJournal in vendorPaymentJournals)
        {
            var partialMappedVendorPayments = vendorPaymentJournal.VendorPayments?
                .Where(p => p.PostingDate.HasValue
                            && !string.IsNullOrWhiteSpace(p.DocumentNumber)
                            && p.Amount > 0
                            && !string.IsNullOrWhiteSpace(p.AppliesToInvoiceNumber))
                ?.ToArray() ?? Array.Empty<DynVendorPayment>();

            foreach (DynVendorPayment partialMappedVendorPayment in partialMappedVendorPayments)
            {
                var dynPurchaseInvoice = purchaseInvoices.FirstOrDefault(purchaseInvoice => purchaseInvoice.Number?.Equals(partialMappedVendorPayment.AppliesToInvoiceNumber) ?? false);

                if (dynPurchaseInvoice is null)
                {
                    continue;
                }

                var paymentRequest = Dynamics365Mapper.Map(partialMappedVendorPayment, vendorPaymentJournal,
                    dynPurchaseInvoice,
                    companyInfoResult.Value, _timeProvider.UtcNow);

                paymentRequests.Add(paymentRequest);
            }
        }

        return paymentRequests.ToArray();
    }
}
