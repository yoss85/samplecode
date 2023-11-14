namespace Dynamics365BC.Models;

[ExcludeFromCodeCoverage]
public sealed class DynPurchaseReceiptLine
{
    /// <summary>
    /// The unique ID of the purchase receipt line. Non-editable.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// The ID of the parent purchase receipt line.
    /// </summary>
    public Guid DocumentId { get; set; }

    /// <summary>
    /// The line sequence number.
    /// </summary>
    public int Sequence { get; set; }

    /// <summary>
    /// The type of the purchase receipt line. It can be " ", "G/L Account", "Item", "Resource", "Fixed Asset" or "Charge Item".
    /// </summary>
    public string? LineType { get; set; }

    /// <summary>
    /// The number of the object (account or item) of the purchase receipt line.
    /// </summary>
    public string? LineObjectNumber { get; set; }

    /// <summary>
    /// Specifies the description of the purchase receipt line.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// The code of unit of measure for the purchase receipt line.
    /// </summary>
    public string? UnitOfMeasureCode { get; set; }

    /// <summary>
    /// The unit cost of each individual item in the purchase receipt line.
    /// </summary>
    public decimal UnitCost { get; set; }

    /// <summary>
    /// The quantity of the item in the purchase receipt line.
    /// </summary>
    public decimal Quantity { get; set; }

    /// <summary>
    /// The line discount percent.
    /// </summary>
    public decimal DiscountPercent { get; set; }

    /// <summary>
    /// The tax percent for the line. Read-Only.
    /// </summary>
    public decimal TaxPercent { get; set; }

    /// <summary>
    /// The date the item in the line is expected to be received.
    /// </summary>
    public DateTime ExpectedReceiptDate { get; set; }

    /// <summary>
    /// Purchase receipt number coming from D365.
    /// </summary>
    public string? Number { get; set; }
}