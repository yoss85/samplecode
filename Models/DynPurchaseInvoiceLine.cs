namespace Dynamics365BC.Models;

[ExcludeFromCodeCoverage]
public sealed class DynPurchaseInvoiceLine
{
    /// <summary>
    /// The unique ID of the purchase invoice line. Non-editable.
    /// </summary>
    public Guid? Id { get; set; }

    /// <summary>
    /// The ID of the parent purchase invoice line.
    /// </summary>
    public Guid? DocumentId { get; set; }

    /// <summary>
    /// The line sequence number.
    /// </summary>
    public int Sequence { get; set; }

    /// <summary>
    /// The ID of the item in the purchase invoice line.
    /// </summary>
    public Guid? ItemId { get; set; }

    /// <summary>
    /// The id of the account that the purchase invoice line is related to.
    /// </summary>
    public Guid? AccountId { get; set; }

    /// <summary>
    /// The type of the purchase invoice line. It can be "Comment", "Account", "Item", "Resource", "Value", "Fixed Asset" or "Charge".
    /// </summary>
    public string? LineType { get; set; }

    /// <summary>
    /// The number of the object (account or item) of the purchase invoice line.
    /// </summary>
    public string? LineObjectNumber { get; set; }

    /// <summary>
    /// Specifies the description of the purchase invoice line.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// The ID of unit of measure for the purchase invoice line.
    /// </summary>
    public Guid? UnitOfMeasureId { get; set; }

    /// <summary>
    /// The code of unit of measure for the purchase invoice line.
    /// </summary>
    public string? UnitOfMeasureCode { get; set; }

    /// <summary>
    /// The unit cost of each individual item in the purchase invoice line.
    /// </summary>
    public decimal? UnitCost { get; set; }

    /// <summary>
    /// The quantity of the item in the purchase invoice line.
    /// </summary>
    public decimal? Quantity { get; set; }

    /// <summary>
    /// The purchase invoice line discount amount.
    /// </summary>
    public decimal DiscountAmount { get; set; }

    /// <summary>
    /// The line discount percent.
    /// </summary>
    public decimal DiscountPercent { get; set; }

    /// <summary>
    /// Specifies whether the discount is applied before tax.
    /// </summary>
    public bool DiscountAppliedBeforeTax { get; set; }

    /// <summary>
    /// The line amount excluding the tax. Read-Only.
    /// </summary>
    public decimal AmountExcludingTax { get; set; }

    /// <summary>
    /// The tax code for the line.
    /// </summary>
    public string? TaxCode { get; set; }

    /// <summary>
    /// The tax percent for the line. Read-Only.
    /// </summary>
    public decimal? TaxPercent { get; set; }

    /// <summary>
    /// The total tax amount for the purchase invoice line. Read-Only.
    /// </summary>
    public decimal? TotalTaxAmount { get; set; }

    /// <summary>
    /// The total amount for the line including tax. Read-Only.
    /// </summary>
    public decimal? AmountIncludingTax { get; set; }

    /// <summary>
    /// The purchase invoice line discount allocation is the purchase invoice line discount distributed on the total amount. Read-Only.
    /// </summary>
    public decimal InvoiceDiscountAllocation { get; set; }

    /// <summary>
    /// The net amount is the amount including all discounts (taken from the purchase invoice line). Read-Only.
    /// </summary>
    public decimal NetAmount { get; set; }

    /// <summary>
    /// The net tax amount is the tax amount calculated from net amount. Read-Only.
    /// </summary>
    public decimal NetTaxAmount { get; set; }

    /// <summary>
    /// The net amount including tax is the total net amount including tax. Read-Only.
    /// </summary>
    public decimal NetAmountIncludingTax { get; set; }

    /// <summary>
    /// The date the item in the line is expected to be received.
    /// </summary>
    public DateTime? ExpectedReceiptDate { get; set; }

    /// <summary>
    /// The ID of the item variant in the purchase invoice line.
    /// </summary>
    public Guid? ItemVariantId { get; set; }

    /// <summary>
    /// The ID of the location where the item in the purchase invoice line is expected to be delivered.
    /// </summary>
    public Guid? LocationId { get; set; }
}