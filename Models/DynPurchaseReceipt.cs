namespace Dynamics365BC.Models;

[ExcludeFromCodeCoverage]
public sealed class DynPurchaseReceipt
{
    /// <summary>
    /// The unique ID of the purchase receipt. Non-editable.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Specifies the number of the purchase receipt.
    /// </summary>
    public string? Number { get; set; }

    /// <summary>
    /// The invoice date.
    /// </summary>
    public DateTime InvoiceDate { get; set; }

    /// <summary>
    /// The date that the purchase receipt is posted.
    /// </summary>
    public DateTime PostingDate { get; set; }

    /// <summary>
    /// The date the purchase receipt is due.
    /// </summary>
    public DateTime DueDate { get; set; }

    /// <summary>
    /// Specifies vendor's number.
    /// </summary>
    public string? VendorNumber { get; set; }

    /// <summary>
    /// Specifies vendor's name.
    /// </summary>
    public string? VendorName { get; set; }

    /// <summary>
    /// Pay to name of the purchase receipt.
    /// </summary>
    public string? PayToName { get; set; }

    /// <summary>
    /// Pay to contact.
    /// </summary>
    public string? PayToContact { get; set; }

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
    /// The default currency code for the purchase receipt.
    /// </summary>
    public string? CurrencyCode { get; set; }

    /// <summary>
    /// The number of the order to which the purchase receipt is associated to. Read-Only.
    /// </summary>
    public string? OrderNumber { get; set; }

    /// <summary>
    /// The last datetime the purchase receipt was modified. Read-Only.
    /// </summary>
    public DateTime LastModifiedDateTime { get; set; }

    #region Navigation Properties

    public IEnumerable<DynPurchaseReceiptLine>? PurchaseReceiptLines { get; set; }

    #endregion
}
