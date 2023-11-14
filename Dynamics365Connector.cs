using AvidConnect.DataModel;
using AvidConnect.Framework;
using Dynamics365BC.Business;
using Dynamics365BC.Extensions;
using Dynamics365BC.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Dynamics365BC;

[ExcludeFromCodeCoverage]
public class Dynamics365Connector :
    ConnectorBase, 
    IExportGlobalList<Vendor>,
    IExportGlobalList<EnterpriseCode>,
    IExportCompanyList<PurchaseOrder>,
    IImportLedger<Invoice, LedgerImportResults>,
    IExportGlobalList<PaymentHistory>,
    IExportCompanyList<PaymentRequest>
{
    ServiceProvider _services = null!;

    public override void Initialize(string clientCode)
    {
        base.Initialize(clientCode);

        var serviceCollection = new ServiceCollection();
        RegisterServices(serviceCollection);
        _services = serviceCollection.BuildServiceProvider();
    }

    public void RegisterServices(IServiceCollection services)
    {
        var dynamicsBaseUrl = Config.GetValue(Dynamics365ConfigurationGroupNames.ConnectionGroupName,
            Dynamics365ConfigurationKeys.DynamicsBaseUrl);

        services.AddDynamics365Services(this, dynamicsBaseUrl);
    }

    public IEnumerable<Vendor> GetGlobalList(IEnumerable<Company> companies, ObjectSearchCriteria<Vendor> criteria)
    {
        using var scope = _services.CreateScope();
        IVendorBusiness? business;

        try
        {
            business = scope.ServiceProvider.GetService<IVendorBusiness>() ??
                       throw new InvalidOperationException("Resolved service was null");
        }
        catch (Exception e)
        {
            Logger.Fatal(
                $"Error occurred while trying to resolve service {nameof(IVendorBusiness)}. Error was: {e.Message}");
            throw;
        }

        Result<Vendor[]> vendorsResult;

        try
        {
            vendorsResult = Task.Run(() => business.GetVendorsAsync())
                .GetAwaiter()
                .GetResult();
        }
        catch (Exception e)
        {
            Logger.Error(CreateVendorImportError(e.Message));
            throw;
        }

        if (vendorsResult.IsFailure)
        {
            var errorMessage = CreateVendorImportError(vendorsResult.Error);
            Logger.Error(errorMessage);
            throw new UserFriendlyException(errorMessage);
        }

        return vendorsResult.Value;
    }

    public IEnumerable<EnterpriseCode> GetGlobalList(IEnumerable<Company> companies, ObjectSearchCriteria<EnterpriseCode> criteria)
    {
        using var scope = _services.CreateScope();
        IChartOfAccountBusiness? business;

        try
        {
            business = scope.ServiceProvider.GetService<IChartOfAccountBusiness>() ??
                       throw new InvalidOperationException("Resolved service was null");
        }
        catch (Exception e)
        {
            Logger.Fatal(
                $"Error occurred while trying to resolve service {nameof(IChartOfAccountBusiness)}. Error was: {e.Message}");
            throw;
        }

        List<EnterpriseCode> enterpriseCodes = new();
        try
        {
            var enterpriseCodesResult = Task.Run(() => business.GetChartOfAccountsAsync())
                .GetAwaiter()
                .GetResult();

            if (enterpriseCodesResult.IsFailure)
            {
                var errorMessage = CreateEnterpriseCodesImportError(enterpriseCodesResult.Error);
                throw new UserFriendlyException(errorMessage);
            }

            enterpriseCodes.AddRange(enterpriseCodesResult.Value);
        }
        catch (Exception e)
        {
            Logger.Error(e.Message);
            throw;
        }

        return enterpriseCodes;
    }

    public IEnumerable<PurchaseOrder> GetCompanyList(Company company, ObjectSearchCriteria<PurchaseOrder> criteria)
    {
        using var scope = _services.CreateScope();
        IPurchaseOrderBusiness? business;

        try
        {
            business = scope.ServiceProvider.GetService<IPurchaseOrderBusiness>()
                       ?? throw new InvalidOperationException("Resolved service was null.");
        }
        catch (Exception e)
        {
            Logger.Error($"Error occurred while trying to resolve service {nameof(IPurchaseOrderBusiness)}. Error was: {e.Message}");
            throw;
        }

        Result<PurchaseOrder[]> purchaseOrdersResult;
        try
        {
            purchaseOrdersResult = Task.Run(() => business.GetPurchaseOrdersAsync())
                .GetAwaiter()
                .GetResult();
        }
        catch (Exception e)
        {
            Logger.Error(CreatePurchaseOrdersImportError(e.Message));
            throw;
        }

        if (purchaseOrdersResult.IsFailure)
        {
            var errorMessage = CreatePurchaseOrdersImportError(purchaseOrdersResult.Error);
            Logger.Error(errorMessage);
            throw new UserFriendlyException(errorMessage);
        }

        return purchaseOrdersResult.Value;
    }

    public LedgerImportResults ImportLedger(Company company, IEnumerable<Invoice> items)
    {
        using var scope = _services.CreateScope();
        IInvoiceBusiness? business;

        try
        {
            business = scope.ServiceProvider.GetService<IInvoiceBusiness>() ??
                       throw new InvalidOperationException("Resolved service was null");
        }
        catch (Exception e)
        {
            Logger.Fatal(
                $"Error occurred while trying to resolve service {nameof(IInvoiceBusiness)}. Error was: {e.Message}");
            throw;
        }

        LedgerImportResults results = new();
        try
        {
            foreach (var invoice in items)
            {
                var invoiceExportResult = Task.Run(() => business.ExportInvoice(invoice))
                    .GetAwaiter()
                    .GetResult();

                if (invoiceExportResult.IsFailure)
                {
                    ((IIntegratedObject)invoice).ValidText = invoiceExportResult.Error;
                    invoice.UpdateType = UpdateType.Error;
                    results.FailedInvoices.Add(invoice);
                }
                else
                {
                    ((IIntegratedObject)invoice).ValidText = string.Empty;
                    invoice.UpdateType = UpdateType.Insert;
                    results.SucceededInvoices.Add(invoice);
                }
            }
        }
        catch (Exception e)
        {
            Logger.Error("Error(s) occurred while exporting Invoice(s): " + e.Message);
            throw new UserFriendlyException(e.Message);
        }

        return results;
    }

    public IEnumerable<PaymentHistory> GetGlobalList(IEnumerable<Company> companies, ObjectSearchCriteria<PaymentHistory> criteria)
    {
        using var scope = _services.CreateScope();
        IVendorPaymentHistoryBusiness? business;

        try
        {
            business = scope.ServiceProvider.GetService<IVendorPaymentHistoryBusiness>()
                       ?? throw new InvalidOperationException("Resolved service was null.");
        }
        catch (Exception e)
        {
            Logger.Error($"Error occurred while trying to resolve service {nameof(IVendorPaymentHistoryBusiness)}. Error was: {e.Message}");
            throw;
        }

        Result<PaymentHistory[]> paymentHistoryResults;
        try
        {
            paymentHistoryResults = Task.Run(() => business.GetVendorsPaymentsHistoryAsync())
                .GetAwaiter()
                .GetResult();
        }
        catch (Exception e)
        {
            Logger.Error(CreatePaymentHistoryImportError(e.Message));
            throw;
        }

        if (paymentHistoryResults.IsFailure)
        {
            var errorMessage = CreatePaymentHistoryImportError(paymentHistoryResults.Error);
            Logger.Error(errorMessage);
            throw new UserFriendlyException(errorMessage);
        }

        return paymentHistoryResults.Value;
    }

    public IEnumerable<PaymentRequest> GetCompanyList(Company company, ObjectSearchCriteria<PaymentRequest> criteria)
    {
        using var scope = _services.CreateScope();
        IPaymentRequestBusiness? business;

        try
        {
            business = scope.ServiceProvider.GetService<IPaymentRequestBusiness>()
                       ?? throw new InvalidOperationException("Resolved service was null.");
        }
        catch (Exception e)
        {
            Logger.Error(
                $"Error occurred while trying to resolve service {nameof(IPaymentRequestBusiness)}. Error was: {e.Message}");
            throw;
        }

        Result<PaymentRequest[]> paymentsResult;
        try
        {
            paymentsResult = Task.Run(() => business.GetPaymentRequestsAsync())
                .GetAwaiter()
                .GetResult();


        }
        catch (Exception e)
        {
            Logger.Error(CreatePaymentsImportError(e.Message));
            throw;
        }

        if (paymentsResult.IsFailure)
        {
            var errorMessage = CreatePaymentsImportError(paymentsResult.Error);
            Logger.Error(errorMessage);
            throw new UserFriendlyException(errorMessage);
        }

        return paymentsResult.Value;
    }

    static string CreateVendorImportError(string error) =>
        "Error occurred while importing vendors: " + error;

    static string CreateEnterpriseCodesImportError(string error) =>
        "Error occurred while importing enterprise codes: " + error;

    static string CreatePurchaseOrdersImportError(string error) =>
        "Error occurred while importing purchase orders: " + error;

    static string CreatePaymentHistoryImportError(string error) =>
        "Error occurred while importing payment history: " + error;

    static string CreatePaymentsImportError(string error) 
        => "Error occurred while importing payments: " + error;

    public override string Version => "1.0";

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _services.Dispose();
        }

        base.Dispose(disposing);
    }
}
