namespace Dynamics365BC.Models;

/// <summary>
/// <see href="https://learn.microsoft.com/en-us/dynamics365/business-central/dev-itpro/api-reference/v2.0/resources/dynamics_purchaseinvoice"/>
/// </summary>
[ExcludeFromCodeCoverage]
public sealed class DynPurchaseInvoice
{
    /// <summary>
    ///	The unique ID of the purchase invoice. Non-editable.
    /// </summary>
    public Guid? Id { get; set; }

    /// <summary>
    /// Specifies the number of the purchase invoice.
    /// </summary>
    public string? Number { get; set; }

    /// <summary>
    /// The invoice date.
    /// </summary>
    public DateTime InvoiceDate { get; set; }

    /// <summary>
    /// The date that the purchase invoice is posted. 
    /// </summary>
    public DateTime PostingDate { get; set; }

    /// <summary>
    /// The date the purchase invoice is due.
    /// </summary>
    public DateTime DueDate { get; set; }

    /// <summary>
    /// The vendor sales order reference for the purchase invoice.
    /// </summary>
    public string? VendorInvoiceNumber { get; set; }

    /// <summary>
    /// The unique ID of vendor.
    /// </summary>
    public Guid VendorId { get; set; }

    /// <summary>
    ///	Specifies vendor's number.
    /// </summary>
    public string? VendorNumber { get; set; }

    /// <summary>
    /// Specifies vendor's name.
    /// </summary>
    public string? VendorName { get; set; }

    /// <summary>
    /// Pay to name of the purchase invoice.
    /// </summary>
    public string? PayToName { get; set; }

    /// <summary>
    /// Pay to contact.
    /// </summary>
    public string? PayToContact { get; set; }

    /// <summary>
    /// Pay to vendor id.
    /// </summary>
    public Guid PayToVendorId { get; set; }

    /// <summary>
    /// Pay to vendor number.
    /// </summary>
    public string? PayToVendorNumber { get; set; }

    /// <summary>
    /// Ship to name.
    /// </summary>
    public string? ShipToName { get; set; }

    /// <summary>
    /// Ship to contact.
    /// </summary>
    public string? ShipToContact { get; set; }

    /// <summary>
    /// Buy from address line 1.
    /// </summary>
    public string? BuyFromAddressLine1 { get; set; }

    /// <summary>
    /// Buy from address line 2.
    /// </summary>
    public string? BuyFromAddressLine2 { get; set; }

    /// <summary>
    /// Buy from city.
    /// </summary>
    public string? BuyFromCity { get; set; }

    /// <summary>
    /// Buy from country.
    /// </summary>
    public string? BuyFromCountry { get; set; }

    /// <summary>
    /// Buy from state.
    /// </summary>
    public string? BuyFromState { get; set; }

    /// <summary>
    /// Buy from post code.
    /// </summary>
    public string? BuyFromPostCode { get; set; }

    /// <summary>
    /// Ship to address line 1.
    /// </summary>
    public string? ShipToAddressLine1 { get; set; }

    /// <summary>
    /// Ship to address line 2.
    /// </summary>
    public string? ShipToAddressLine2 { get; set; }

    /// <summary>
    /// Ship to city.
    /// </summary>
    public string? ShipToCity { get; set; }

    /// <summary>
    ///	Ship to country.
    /// </summary>
    public string? ShipToCountry { get; set; }

    /// <summary>
    ///	Ship to state.
    /// </summary>
    public string? ShipToState { get; set; }

    /// <summary>
    /// Ship to post code.
    /// </summary>
    public string? ShipToPostCode { get; set; }

    /// <summary>
    /// Pay to address line 1.
    /// </summary>
    public string? PayToAddressLine1 { get; set; }

    /// <summary>
    /// Pay to address line 2.
    /// </summary>
    public string? PayToAddressLine2 { get; set; }

    /// <summary>
    /// Pay to city
    /// </summary>
    public string? PayToCity { get; set; }

    /// <summary>
    /// Pay to country.
    /// </summary>
    public string? PayToCountry { get; set; }

    /// <summary>
    ///	Pay to state.
    /// </summary>
    public string? PayToState { get; set; }

    /// <summary>
    /// Pay to post code.
    /// </summary>
    public string? PayToPostCode { get; set; }
    public string? ShortcutDimension1Code { get; set; }
    public string? ShortcutDimension2Code { get; set; }

    /// <summary>
    ///	Specifies which currency the purchase invoice uses.
    /// </summary>
    public Guid CurrencyId { get; set; }

    /// <summary>
    /// The default currency code for the purchase invoice.
    /// </summary>
    public string? CurrencyCode { get; set; }

    /// <summary>
    /// The id of the order to which the purchase invoice is associated to. Read-Only.
    /// </summary>
    public Guid? OrderId { get; set; }

    /// <summary>
    /// The number of the order to which the purchase invoice is associated to. Read-Only.
    /// </summary>
    public string? OrderNumber { get; set; }

    /// <summary>
    /// Specifies whether the prices include Tax or not. Read-Only.
    /// </summary>
    public bool PricesIncludeTax { get; set; }

    /// <summary>
    /// The purchase invoice discount amount.
    /// </summary>
    public decimal DiscountAmount { get; set; }

    /// <summary>
    /// Specifies whether the discount is applied before tax.
    /// </summary>
    public bool DiscountAppliedBeforeTax { get; set; }

    /// <summary>
    /// The total amount excluding tax. Read-Only.
    /// </summary>
    public decimal? TotalAmountExcludingTax { get; set; }

    /// <summary>
    ///	The total tax amount for the purchase invoice. Read-Only.
    /// </summary>
    public decimal? TotalTaxAmount { get; set; }

    /// <summary>
    ///	The total amount including tax. Read-Only.
    /// </summary>
    public decimal? TotalAmountIncludingTax { get; set; }

    /// <summary>
    /// The status of the purchase invoice.
    /// It can be " ", "Draft", "In Review", "Open", "Paid", "Canceled" or "Corrective".
    /// <see cref="InvoiceStatus"/>
    /// </summary>
    public string? Status { get; set; }

    /// <summary>
    /// The last datetime the purchase invoice was modified. Read-Only.
    /// </summary>
    public DateTime? LastModifiedDateTime { get; set; }

    #region Navigation Properties

    public DynVendor? Vendor { get; set; }

    #endregion
}

public static class InvoiceStatus
{
    public const string Empty = " ";
    public const string Draft = "Draft";
    public const string InReview = "In Review";
    public const string Open = "Open";
    public const string Paid = "Paid";
    public const string Cancelled = "Cancelled";
    public const string Corrective = "Corrective";
}
