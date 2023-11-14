namespace Dynamics365BC.Models;

/// <summary>
/// <see href="https://learn.microsoft.com/en-us/dynamics365/business-central/dev-itpro/api-reference/v2.0/resources/dynamics_vendorpaymentjournal"/>
/// </summary>
[ExcludeFromCodeCoverage]
public sealed class DynVendorPaymentJournal
{
    /// <summary>
    /// The unique ID of the vendor payment journal. Non-editable.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// The code of the vendor payment journal.
    /// </summary>
    public string? Code { get; set; }

    /// <summary>
    /// Specifies the vendor payment journal's name.
    /// This name will appear on all sales documents for the vendor payment journal.
    /// </summary>
    public string? DisplayName { get; set; }

    /// <summary>
    /// The balancing G/L Account ID.
    /// </summary>
    public Guid BalancingAccountId { get; set; }

    /// <summary>
    ///	The balancing G/L Account number.
    /// </summary>
    public string? BalancingAccountNumber { get; set; }

    /// <summary>
    /// The last datetime the vendor payment journal was modified. Read-Only.
    /// </summary>
    public DateTime LastModifiedDateTime { get; set; }

    #region Navigation Properties

    public IEnumerable<DynVendorPayment>? VendorPayments { get; set; }

    #endregion
}
