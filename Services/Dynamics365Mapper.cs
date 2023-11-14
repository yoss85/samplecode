using AvidConnect.DataModel;
using Dynamics365BC.Models;

namespace Dynamics365BC.Services;

public sealed class Dynamics365Mapper
{
    public static Vendor Map(DynVendor dynamicsVendor)
    {
        var result = new Vendor();
        result.ExternalVendorCode = dynamicsVendor.Id.ToString();

        result.FedTaxId = dynamicsVendor.TaxRegistrationNumber;
        result.NameOnCheck = dynamicsVendor.DisplayName;
        result.Name = dynamicsVendor.DisplayName;

        Address vendorAddress = new()
        {
            Line1 = dynamicsVendor.AddressLine1,
            Line2 = dynamicsVendor.AddressLine2,
            City = dynamicsVendor.City,
            State = dynamicsVendor.State,
            Country = dynamicsVendor.Country,
            PostalCode = dynamicsVendor.PostalCode
        };
        result.Address = vendorAddress;

        result.Telephone1 = dynamicsVendor.PhoneNumber;
        result.Email = dynamicsVendor.Email;
        result.Website = dynamicsVendor.Website;

        result.LastModifiedDate = dynamicsVendor.LastModifiedDateTime;
        result.IsValid = true;

        string? vendorBlocked = dynamicsVendor.Blocked?.Replace("_x0020_", " ")?.Trim();
        result.Active = string.IsNullOrWhiteSpace(vendorBlocked);

        return result;
    }

    public static EnterpriseCode Map(DynAccount dynamicsAccount)
    {
        EnterpriseCode accountEnterpriseCode = new();

        accountEnterpriseCode.CodeValue = dynamicsAccount.Id.ToString();
        accountEnterpriseCode.Description = $"{dynamicsAccount.Number} : {dynamicsAccount.DisplayName}";
        accountEnterpriseCode.GroupName = "Accounting Codes";
        accountEnterpriseCode.IsActive = !dynamicsAccount.Blocked;
        return accountEnterpriseCode;
    }

    public static IEnumerable<EnterpriseCode> Map(DynDimension dynamicsDimension)
    {
        List<EnterpriseCode> enterpriseCodes = new();

        dynamicsDimension.DimensionValues ??= Array.Empty<DynDimensionValue>();
        foreach (var dimensionValue in dynamicsDimension.DimensionValues)
        { 
            EnterpriseCode dimensionValEnterpriseCode = new();

            dimensionValEnterpriseCode.CodeValue = dimensionValue.Code?.Trim();
            dimensionValEnterpriseCode.Description = dimensionValue.DisplayName?.Trim();
            dimensionValEnterpriseCode.GroupName = dynamicsDimension.DisplayName?.Trim();
            dimensionValEnterpriseCode.IsActive = true;
            enterpriseCodes.Add(dimensionValEnterpriseCode);
        }

        return enterpriseCodes.AsEnumerable();
    }

    public static Result<(DynPurchaseInvoice invoice, DynPurchaseInvoiceLine[] lineItems)> Map(Invoice invoice,
        string currencyCodeGroupName)
    {
        DynPurchaseInvoice dynPurchaseInvoice = new();

        dynPurchaseInvoice.VendorInvoiceNumber = invoice.InvoiceNumber;
        dynPurchaseInvoice.InvoiceDate = invoice.InvoiceDate ?? default;
        dynPurchaseInvoice.PostingDate = invoice.PostingDate ?? default;
        dynPurchaseInvoice.DueDate = invoice.DueDate ?? default;

        if (!Guid.TryParse(invoice.VendorExternalCode, out var externalVendorId))
        {
            return Result.Failure<(DynPurchaseInvoice, DynPurchaseInvoiceLine[])>("Error mapping from Invoice to DynPurchaseInvoice. Invoice.VendorExternalCode should be a valid Guid");
        }

        dynPurchaseInvoice.VendorId = externalVendorId;
        dynPurchaseInvoice.PayToVendorId = externalVendorId;
        
        // extract VendorName and VendorNumber from Invoice.VendorName
        // convention is: '<vendor number> | <vendor name>'
        var data = invoice.VendorName.Split('|');
        dynPurchaseInvoice.VendorNumber = data[0].Trim();

        if (data.Length > 1)
            dynPurchaseInvoice.VendorName = data[1].Trim();

        dynPurchaseInvoice.TotalAmountIncludingTax = invoice.Amount;

        dynPurchaseInvoice.CurrencyCode = "USD";

        dynPurchaseInvoice.ShipToName = invoice.CompanyName;
        
        // address
        dynPurchaseInvoice.ShipToAddressLine1 = invoice.CompanyAddress?.Line1;
        dynPurchaseInvoice.ShipToAddressLine2 = invoice.CompanyAddress?.Line2;
        dynPurchaseInvoice.ShipToCity = invoice.CompanyAddress?.City;
        dynPurchaseInvoice.ShipToState = invoice.CompanyAddress?.State;
        dynPurchaseInvoice.ShipToCountry = invoice.CompanyAddress?.Country;
        dynPurchaseInvoice.ShipToPostCode = invoice.CompanyAddress?.PostalCode;

        List<DynPurchaseInvoiceLine> dynInvoiceLines = new();
        foreach (var line in invoice.Lines)
        {
            if (line is null) continue;

            var description = string.Empty;

            DynPurchaseInvoiceLine dynInvoiceLine = new();
            dynInvoiceLine.Sequence = line.LineNumber;

            if (!Guid.TryParse(line?.GLCode?.CodeValue, out var accountId))
                dynInvoiceLine.AccountId = accountId;

            if (Guid.TryParse(line?.Item?.ProductExternalCode, out var itemId))
                dynInvoiceLine.ItemId = itemId;

            if (!string.IsNullOrWhiteSpace(invoice.AccountNumber))
                description = $"{invoice.AccountNumber}";

            if (!string.IsNullOrWhiteSpace(invoice.Reference))
                description = !string.IsNullOrWhiteSpace(description) ? $"{description}|{invoice.Reference}" : $"{invoice.Reference}";
                
            if (!string.IsNullOrWhiteSpace(line!.Item?.ProductName))
                description = !string.IsNullOrWhiteSpace(description) ? $"{description}|{line.Item?.ProductName}" : $"{line.Item?.ProductName}";

            dynInvoiceLine.AmountIncludingTax = line.Amount;
            dynInvoiceLine.Description = description;
            dynInvoiceLine.UnitCost = line.Item?.PricePer;
            dynInvoiceLine.UnitOfMeasureCode = line.Item?.UOM;
            dynInvoiceLine.Quantity = line.Item?.Quantity;
                
            dynInvoiceLines.Add(dynInvoiceLine);
        }

        return Result.Success((dynPurchaseInvoice, dynInvoiceLines.ToArray()));
    }

    public static PurchaseOrder Map(DynPurchaseOrder dynamicsPurchaseOrder)
    {
        PurchaseOrder purchaseOrder = new();

        purchaseOrder.ExternalCode = dynamicsPurchaseOrder.Id.ToString();
        purchaseOrder.OrderNumber = dynamicsPurchaseOrder.Number;
        purchaseOrder.RequisitionName = dynamicsPurchaseOrder.Purchaser;
        purchaseOrder.VendorExternalCode = dynamicsPurchaseOrder.VendorId.ToString();

        if (dynamicsPurchaseOrder.PurchaseOrderLines?.Any() ?? false)
        {
            var locationId = GetLocationId(dynamicsPurchaseOrder.PurchaseOrderLines);
            purchaseOrder.BillToCompanyExternalCode = locationId.ToString();
            purchaseOrder.ShipToCompanyExternalCode = locationId.ToString();
        }

        purchaseOrder.EndDate = dynamicsPurchaseOrder.RequestedReceiptDate;
        purchaseOrder.ShippingMethod = dynamicsPurchaseOrder.ShipmentMethodId.ToString();
        purchaseOrder.Taxes = dynamicsPurchaseOrder.TotalTaxAmount;
        purchaseOrder.Discount = dynamicsPurchaseOrder.DiscountAmount;
        purchaseOrder.TotalAmount = dynamicsPurchaseOrder.TotalAmountIncludingTax;

        purchaseOrder.LineItems = (dynamicsPurchaseOrder?.PurchaseOrderLines ?? Array.Empty<DynPurchaseOrderLine>())
            .Select(dynLine => new PurchaseOrderLine
            {
                ExternalCode = dynLine.Id.ToString(),
                Item = new PurchaseItem
                {
                    ProductDesc = dynLine.Description,
                    ProductCode = dynLine.ItemId.ToString(),
                    Quantity = dynLine.Quantity,
                    UOM = dynLine.UnitOfMeasureCode,
                },
                Amount = dynLine.AmountIncludingTax,
                PricePer = dynLine.DirectUnitCost
            }).ToList();

        return purchaseOrder;

        // Get LocationId from any line that has a valid LocationId
        // Returns: Guid.Empty if no such dynPOLIne is found, valid Guid otherwise
        static Guid GetLocationId(IEnumerable<DynPurchaseOrderLine> dynPOLines)
            => dynPOLines?.FirstOrDefault(pol => pol.LocationId != Guid.Empty)?.LocationId ?? Guid.Empty;
    }

    public static PaymentHistory Map(DynVendorPayment dynamicsVendorPayment)
    {
        PaymentHistory paymentHistory = new();

        paymentHistory.ExternalCode = dynamicsVendorPayment.Id.ToString();
        paymentHistory.PaymentNumber = dynamicsVendorPayment.DocumentNumber;
        paymentHistory.PaymentDate = dynamicsVendorPayment.PostingDate;
        paymentHistory.PaymentAmount = dynamicsVendorPayment.Amount;
        paymentHistory.PayerBankAccountNumber = string.Empty;

        paymentHistory.PaymentState = PaymentHistoryState.Outstanding;
        paymentHistory.RemittanceType = RemittanceType.Check;

        paymentHistory.BatchExternalCode = $"{dynamicsVendorPayment.DocumentNumber}-{dynamicsVendorPayment.AppliesToInvoiceNumber}-{dynamicsVendorPayment.VendorNumber}";
        paymentHistory.PayeeExternalCode = dynamicsVendorPayment.VendorId.ToString();
        paymentHistory.PaymentState = PaymentHistoryState.Outstanding;

        paymentHistory.Invoices = new List<PaymentHistoryInvoice>();

        PaymentHistoryInvoice invoiceItem = new()
        {
            InvoiceNumber = dynamicsVendorPayment.AppliesToInvoiceNumber,
            InvoiceAmount = dynamicsVendorPayment.Amount
        };

        invoiceItem.InvoiceNumber = dynamicsVendorPayment.ExternalDocumentNumber;
        paymentHistory.Invoices.Add(invoiceItem);

        return paymentHistory;
    }

    public static MaterialReceipt[] Map(DynPurchaseReceipt dynamicsPurchaseReceipt)
    {
        return (dynamicsPurchaseReceipt?.PurchaseReceiptLines ?? Array.Empty<DynPurchaseReceiptLine>())
        .Select(dynReceiptLine => new MaterialReceipt
        {
            ExternalCode = dynamicsPurchaseReceipt?.Id.ToString(),
            PurchaseOrderNumber = dynamicsPurchaseReceipt?.OrderNumber,
            PurchaseOrderLineNumber = dynReceiptLine.Sequence,
            ProductCode = dynReceiptLine.LineObjectNumber,
            ProductName = dynReceiptLine.Description,
            UOM = dynReceiptLine.UnitOfMeasureCode,
            DateReceived = dynReceiptLine.ExpectedReceiptDate,
            Quantity = dynReceiptLine.Quantity,
            BillOfLadingNumber = dynamicsPurchaseReceipt?.Number
        }).ToArray();
    }

    public static PaymentRequest Map(DynVendorPayment dynamicsVendorPayment,
        DynVendorPaymentJournal dynamicsVendorPaymentJournal,
        DynPurchaseInvoice dynamicsPurchaseInvoice,
        DynCompanyInformation dynamicsCompanyInfo,
        DateTime batchExternalCodeTime)
    {
        PaymentRequestInvoice paymentRequestInvoice = new ()
        {
            InvoiceNumber = dynamicsPurchaseInvoice.VendorInvoiceNumber,
            InvoiceDate = dynamicsPurchaseInvoice.InvoiceDate,
            InvoiceDueDate = dynamicsPurchaseInvoice.DueDate,
            InvoiceGrossAmount = dynamicsPurchaseInvoice.TotalAmountIncludingTax,
            InvoiceNetAmount = dynamicsPurchaseInvoice.TotalAmountIncludingTax ?? 0.0m
        };

        var map = new PaymentRequest();
        map.BatchExternalCode = $"{dynamicsVendorPaymentJournal?.Code}--{batchExternalCodeTime:dd-MMM-yyy-HH'-'mm'-'ss}";
        map.PaymentExternalCode = dynamicsVendorPayment?.Id.ToString();
        map.PaymentNumber = dynamicsVendorPayment?.DocumentNumber;
        map.PaymentDate = dynamicsVendorPayment?.PostingDate ?? default;
        map.PaymentAmount = dynamicsVendorPayment?.Amount ?? 0;
        map.PayerExternalCode = dynamicsCompanyInfo.Id.ToString();
        map.PayerName = dynamicsCompanyInfo.DisplayName;
        map.PayerAddress = new Address
        {
            Line1 = dynamicsCompanyInfo.AddressLine1,
            Line2 = dynamicsCompanyInfo.AddressLine2,
            City = dynamicsCompanyInfo.City,
            State = dynamicsCompanyInfo.State,
            PostalCode = dynamicsCompanyInfo.PostalCode
        };
        map.PayerBankAccountId = dynamicsVendorPaymentJournal?.BalancingAccountNumber;
        map.PayeeExternalCode = dynamicsVendorPayment?.VendorNumber;
        map.PayeeName = dynamicsVendorPayment?.Description;
        map.PayeeNameOnCheck = dynamicsVendorPayment?.Description;
        map.PayeeAddress = new Address
        {
            Line1 = dynamicsPurchaseInvoice.Vendor?.AddressLine1,
            Line2 = dynamicsPurchaseInvoice.Vendor?.AddressLine2,
            City = dynamicsPurchaseInvoice.Vendor?.City,
            State = dynamicsPurchaseInvoice.Vendor?.State,
            PostalCode = dynamicsPurchaseInvoice.Vendor?.PostalCode
        };
        map.PayeeRemitAddress = new Address
        {
            Line1 = dynamicsPurchaseInvoice.PayToAddressLine1,
            Line2 = dynamicsPurchaseInvoice.PayToAddressLine2,
            City = dynamicsPurchaseInvoice.PayToCity,
            State = dynamicsPurchaseInvoice.PayToState,
            PostalCode = dynamicsPurchaseInvoice.PayToPostCode
        };
        map.Invoices = new List<PaymentRequestInvoice> { paymentRequestInvoice };
        return map;
    }
}