namespace Dynamics365BC.Models;

/// <summary>
/// <see cref="https://learn.microsoft.com/en-us/dynamics365/business-central/dev-itpro/api-reference/v2.0/resources/dynamics_company"/>
/// </summary>
[ExcludeFromCodeCoverage]
public sealed class DynCompany
{
    /// <summary>
    /// The unique ID of the company. Non-editable.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Specifies the internal version of the company.
    /// </summary>
    public string? SystemVersion { get; set; }

    public long TimeStamp { get; set; }

    /// <summary>
    ///	Represents the company's name.
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Specifies the company's name. This name will appear on all sales documents for the company.
    /// </summary>
    public string? DisplayName { get; set; }

    /// <summary>
    /// Specifies the Business Profile ID linked to the company.
    /// </summary>
    public string? BusinessProfileId { get; set; }

    /// <summary>
    /// The datetime the company was created.
    /// </summary>
    public DateTime SystemCreatedAt { get; set; }

    /// <summary>
    /// The ID of the user who created the company.
    /// </summary>
    public Guid SystemCreatedBy { get; set; }

    /// <summary>
    /// The last datetime the company was modified.
    /// </summary>
    public DateTime SystemModifiedAt { get; set; }

    /// <summary>
    /// The ID of the user who last modified the company.
    /// </summary>
    public Guid SystemModifiedBy { get; set; }
}
