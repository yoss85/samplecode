namespace Dynamics365BC.Models;

[ExcludeFromCodeCoverage]
public sealed class DynPurchaseOrder
{
    /// <summary>
    /// The unique ID of the purchase order. Non-editable.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Specifies the number of the purchase order.
    /// </summary>
    public string? Number { get; set; }

    /// <summary>
    /// The order date.
    /// </summary>
    public DateTime OrderDate { get; set; }

    /// <summary>
    /// The date that the purchase order is posted.
    /// </summary>
    public DateTime PostingDate { get; set; }

    /// <summary>
    /// The unique ID of the vendor.
    /// </summary>
    public Guid VendorId { get; set; }

    /// <summary>
    /// Specifies the vendor's number.
    /// </summary>
    public string? VendorNumber { get; set; }

    /// <summary>
    /// Specifies the vendor's name.
    /// </summary>
    public string? VendorName { get; set; }

    /// <summary>
    /// Pay to name of the purchase order.
    /// </summary>
    public string? PayToName { get; set; }

    /// <summary>
    /// Pay to vendor ID.
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
    /// Pay to address line 1.
    /// </summary>
    public string? PayToAddressLine1 { get; set; }

    /// <summary>
    /// Pay to address line 2.
    /// </summary>
    public string? PayToAddressLine2 { get; set; }

    /// <summary>
    /// Pay to city.
    /// </summary>
    public string? PayToCity { get; set; }

    /// <summary>
    /// Pay to country.
    /// </summary>
    public string? PayToCountry { get; set; }

    /// <summary>
    /// Pay to state.
    /// </summary>
    public string? PayToState { get; set; }

    /// <summary>
    /// Pay to post code.
    /// </summary>
    public string? PayToPostCode { get; set; }

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
    /// Ship to country.
    /// </summary>
    public string? ShipToCountry { get; set; }

    /// <summary>
    /// Ship to state.
    /// </summary>
    public string? ShipToState { get; set; }

    /// <summary>
    /// Ship to post code.
    /// </summary>
    public string? ShipToPostCode { get; set; }

    /// <summary>
    /// Specifies the first shortcut dimension code.
    /// </summary>
    public string? ShortcutDimension1Code { get; set; }

    /// <summary>
    /// Specifies the second shortcut dimension code.
    /// </summary>
    public string? ShortcutDimension2Code { get; set; }

    /// <summary>
    /// Specifies which currency the purchase order uses.
    /// </summary>
    public Guid CurrencyId { get; set; }

    /// <summary>
    /// The default currency code for the purchase order.
    /// </summary>
    public string? CurrencyCode { get; set; }

    /// <summary>
    /// Specifies whether the prices include tax or not. Read-Only.
    /// </summary>
    public bool PricesIncludeTax { get; set; }

    /// <summary>
    /// Specifies which payment term the purchase order uses.
    /// </summary>
    public Guid PaymentTermsId { get; set; }

    /// <summary>
    /// Specifies which shipment method the purchase order uses.
    /// </summary>
    public Guid ShipmentMethodId { get; set; }

    /// <summary>
    /// The purchaser in the purchase order.
    /// </summary>
    public string? Purchaser { get; set; }

    /// <summary>
    /// The date the receipt was requested.
    /// </summary>
    public DateTime RequestedReceiptDate { get; set; }

    /// <summary>
    /// The purchase order discount amount.
    /// </summary>
    public decimal DiscountAmount { get; set; }

    /// <summary>
    /// Specifies whether the discount is applied before tax.
    /// </summary>
    public bool DiscountAppliedBeforeTax { get; set; }

    /// <summary>
    /// The total amount excluding tax. Read-Only.
    /// </summary>
    public decimal TotalAmountExcludingTax { get; set; }

    /// <summary>
    /// The total tax amount for the purchase order. Read-Only.
    /// </summary>
    public decimal TotalTaxAmount { get; set; }

    /// <summary>
    /// The total amount including tax. Read-Only.
    /// </summary>
    public decimal TotalAmountIncludingTax { get; set; }

    /// <summary>
    /// Specifies whether the purchase order has been fully received.
    /// </summary>
    public bool FullyReceived { get; set; }

    /// <summary>
    /// The status of the purchase order. It can be "Draft", "In Review" or "Open".
    /// <see cref="PurchaseOrderStatus"/>
    /// </summary>
    public string? Status { get; set; }

    /// <summary>
    /// The last datetime the purchase order was modified. Read-Only.
    /// </summary>
    public DateTime LastModifiedDateTime { get; set; }

    #region Navigation Properties

    public IEnumerable<DynPurchaseOrderLine>? PurchaseOrderLines { get; set; }

    #endregion
}

/// <summary>
/// Represents the status of the purchase order.
/// </summary>
public static class PurchaseOrderStatus
{
    public const string Draft = "Draft";
    public const string InReview = "In Review";
    public const string Open = "Open";
}
