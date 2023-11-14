namespace Dynamics365BC.Models;

[ExcludeFromCodeCoverage]
public sealed class DynCompanyInformation
{
    /// <summary>
    /// The unique ID of the company information. Non-editable.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Specifies the company information's name. This name will appear on all sales documents for the company information.
    /// </summary>
    public string? DisplayName { get; set; }

    /// <summary>
    /// Specifies the company information's address. This address will appear on all sales documents for the company information.
    /// </summary>
    public string? AddressLine1 { get; set; }

    /// <summary>
    /// Specifies the company information's address. This address will appear on all sales documents for the company information.
    /// </summary>
    public string? AddressLine2 { get; set; }

    /// <summary>
    /// Specifies the company information's city.
    /// </summary>
    public string? City { get; set; }

    /// <summary>
    /// Specifies the company information's state.
    /// </summary>
    public string? State { get; set; }

    /// <summary>
    /// Specifies the company information's country.
    /// </summary>
    public string? Country { get; set; }

    /// <summary>
    /// Specifies the company information's postal code.
    /// </summary>
    public string? PostalCode { get; set; }

    /// <summary>
    /// Specifies the company information's telephone number.
    /// </summary>
    public string? PhoneNumber { get; set; }

    /// <summary>
    /// The company's fax number.
    /// </summary>
    public string? FaxNumber { get; set; }

    /// <summary>
    /// Specifies the company information's email address.
    /// </summary>
    public string? Email { get; set; }
    /// <summary>
    /// Specifies the company information's home page address.
    /// </summary>
    public string? Website { get; set; }

    /// <summary>
    /// Specified the tax registration number of the company information.
    /// </summary>
    public string? TaxRegistrationNumber { get; set; }

    /// <summary>
    /// The default currency code for the company information.
    /// </summary>
    public string? CurrencyCode { get; set; }

    /// <summary>
    /// The company's current fiscal year start date. Read-Only.
    /// </summary>
    public DateTime CurrentFiscalYearStartDate { get; set; }

    /// <summary>
    /// The industry the company is part of.
    /// </summary>
    public string? Industry { get; set; }

    /// <summary>
    /// The company information's picture.
    /// </summary>
    public string? Picture { get; set; }

    /// <summary>
    /// The last datetime the company information was modified. Read-Only.
    /// </summary>
    public DateTime LastModifiedDateTime { get; set; }
}