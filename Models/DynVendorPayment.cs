namespace Dynamics365BC.Models;

/// <summary>
/// <see href="https://learn.microsoft.com/en-us/dynamics365/business-central/dev-itpro/api-reference/v2.0/resources/dynamics_vendorpayment"/>
/// </summary>
[ExcludeFromCodeCoverage]
public sealed class DynVendorPayment
{
    /// <summary>
    /// The unique ID of the vendor payment. Non-editable.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// The ID of the journal.
    /// </summary>
    public Guid JournalId { get; set; }

    /// <summary>
    /// The display name of the journal that this line belongs to. Read-Only.
    /// </summary>
    public string? JournalDisplayName { get; set; }

    /// <summary>
    /// The vendor payment item line number.
    /// </summary>
    public int LineNumber { get; set; }

    /// <summary>
    ///	The unique ID of vendor.
    /// </summary>
    public Guid VendorId { get; set; }

    /// <summary>
    /// Specifies vendor's number.
    /// </summary>
    public string? VendorNumber { get; set; }

    /// <summary>
    /// The date that the vendor payment is posted.
    /// </summary>
    public DateTime? PostingDate { get; set; }

    /// <summary>
    ///	Specifies a document number for the vendor payment.
    /// </summary>
    public string? DocumentNumber { get; set; }

    /// <summary>
    /// Specifies an external document number for the vendor payment.
    /// </summary>
    public string? ExternalDocumentNumber { get; set; }

    /// <summary>
    ///	Specifies the total amount (including VAT) that the vendor payment consists of.
    /// </summary>
    public decimal Amount { get; set; }

    /// <summary>
    /// The unique ID of the invoice that the vendor payment is related to.
    /// </summary>
    public Guid AppliesToInvoiceId { get; set; }

    /// <summary>
    /// The number of the invoice that the vendor payment is related to.
    /// </summary>
    public string? AppliesToInvoiceNumber { get; set; }

    /// <summary>
    /// Specifies the description of the vendor payment.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// A user specified comment on the vendor payment.
    /// </summary>
    public string? Comment { get; set; }

    /// <summary>
    /// The last datetime the vendor payment was modified. Read-Only.
    /// </summary>
    public DateTime LastModifiedDateTime { get; set; }

}
