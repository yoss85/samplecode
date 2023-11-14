namespace Dynamics365BC.Models;

/// <summary>
/// Based on Microsoft Dynamics365 Business Central Vendor Entity
/// <see cref="https://learn.microsoft.com/en-us/dynamics365/business-central/dev-itpro/api-reference/v2.0/resources/dynamics_vendor"/>
/// </summary>
[ExcludeFromCodeCoverage]
public sealed class DynVendor
{
    /// <summary>
    /// The unique ID of the vendor. Non-editable.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Specifies the number of the vendor.
    /// </summary>
    public string? Number { get; set; }

    /// <summary>
    /// Specifies the vendor's name. This name will appear on all sales documents for the vendor.
    /// </summary>
    public string? DisplayName { get; set; }

    /// <summary>
    /// Specifies the vendor's address. This address will appear on all sales documents for the vendor.
    /// </summary>
    public string? AddressLine1 { get; set; }

    /// <summary>
    /// Specifies the vendor's address. This address will appear on all sales documents for the vendor.
    /// </summary>
    public string? AddressLine2 { get; set; }

    /// <summary>
    /// Specifies the vendor's city.
    /// </summary>
    public string? City { get; set; }

    /// <summary>
    /// Specifies the vendor's state.
    /// </summary>
    public string? State { get; set; }

    /// <summary>
    /// Specifies the vendor's country.
    /// </summary>
    public string? Country { get; set; }

    /// <summary>
    /// Specifies the vendor's postal code.
    /// </summary>
    public string? PostalCode { get; set; }

    /// <summary>
    /// Specifies the vendor's telephone number.
    /// </summary>
    public string? PhoneNumber { get; set; }

    /// <summary>
    /// Specifies the vendor's email address.
    /// </summary>
    public string? Email { get; set; }

    /// <summary>
    /// Specifies the vendor's home page address.
    /// </summary>
    public string? Website { get; set; }

    /// <summary>
    /// Specified the tax registration number of the vendor.
    /// </summary>
    public string? TaxRegistrationNumber { get; set; }

    /// <summary>
    /// Specifies which currency the vendor uses.
    /// </summary>
    public Guid CurrencyId { get; set; }

    /// <summary>
    ///	The default currency code for the vendor.
    /// </summary>
    public string? CurrencyCode { get; set; }

    /// <summary>
    /// Specifies a 1099 code for the vendor. US only.
    /// </summary>
    public string? Irs1099Code { get; set; }

    /// <summary>
    /// Specifies which payment term the vendor uses.
    /// </summary>
    public Guid PaymentTermsId { get; set; }

    /// <summary>
    /// Specifies which payment method the vendor uses.
    /// </summary>
    public Guid PaymentMethodId { get; set; }

    /// <summary>
    /// Specifies if the vendor or vendor is liable for sales tax. Set to true if the vendor is tax liable.
    /// </summary>
    public bool TaxLiable { get; set; }

    /// <summary>
    /// Specifies which transactions with the customer cannot be posted.
    /// It can be: Member Name=" ", "Payment" or "All".
    /// </summary>
    public string? Blocked { get; set; }

    /// <summary>
    /// Specifies vendor's total balance.
    /// </summary>
    public decimal Balance { get; set; }

    /// <summary>
    /// The last datetime the vendor was modified. Read-Only.
    /// </summary>
    public DateTime LastModifiedDateTime { get; set; }
}